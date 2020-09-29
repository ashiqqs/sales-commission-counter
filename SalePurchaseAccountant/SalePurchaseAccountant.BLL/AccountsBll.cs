using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using SalePurchaseAccountant.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class AccountsBll
    {
        private readonly PolicyBll _policy;
        private readonly SalaryGetway _salary;
        private readonly SalesmanGetway _salesman;
        private readonly MemberGetway _member;
        public AccountsBll()
        {
            _policy = new PolicyBll();
            _salary = new SalaryGetway();
            _salesman = new SalesmanGetway();
            _member = new MemberGetway();
        }

        public bool ProcessSalary()
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                using(var tran = con.BeginTransaction())
                {
                    try
                    {
                        var company = _salesman.Get("E-001").FirstOrDefault();
                        var topLevelSalesman = _salesman.GetAssociates(company.Code);
                        _salary.CreateNewSalaryAcc(con, tran);
                        ProcessSalesBoundGbCommission(con, tran, topLevelSalesman);
                        ProcessOrdinalCommission(con, tran);
                        tran.Commit();
                        return true;
                    }
                    catch(Exception err)
                    {
                        tran.Rollback();
                        throw (err);
                    }
                }
            }
        }
        public List<SalaryViewModel> GetSalary(string code, string month)
        {
            return _salary.GetSalary(code, month);
        }
        private void ProcessSalesBoundGbCommission(SqlConnection con, SqlTransaction tran, List<SalesmanModel> salesmanList)
        {
            foreach(var salesman in salesmanList)
            {
                double salesAmount = _salesman.GetSalesAmount(code:salesman.Code);
                if (salesman.IsAlphaMember)
                {
                    double inboundCommissionPercent = _policy.InboundCommissionPercentage(salesAmount);
                    double inboundCommission = salesAmount * inboundCommissionPercent / 100;
                    _salary.AddInboundCommission(con, tran, salesman.Code, inboundCommission);
                }
                if (salesman.IsBetaMember)
                {
                    double outboundCommissionPercent = _policy.OutboundCommissionPercentage(salesAmount);
                    double outboundCommission = salesAmount * outboundCommissionPercent / 100;
                    _salary.AddOutboundCommission(con, tran, salesman.Code, outboundCommission);

                }
                ProcessSalesCommission(con, tran, salesman, salesAmount);
                ProcessGbCommission(con, tran, salesman);
                var associates = _salesman.GetAssociates(salesman.Code);
                ProcessSalesBoundGbCommission(con, tran, associates);
            }
        }
        private void ProcessSalesCommission(SqlConnection con, SqlTransaction tran,SalesmanModel salesman, double salesAmount, double paidCommissionPercent = 0)
        {
            if (paidCommissionPercent == 26) { return; }
                double personalSalesAmount = _salesman.GetSalesAmount(code: salesman.Code);
                double selfCommissionPercent =_policy.SalesCommissionPercentage(salesman.Designaiton, salesAmount);
                double payableAmount = salesAmount * (selfCommissionPercent - paidCommissionPercent) / 100;

                if (_policy.IsEligible(personalSalesAmount))
                {
                    _salary.AddSalesCommission(con, tran, salesman.Code, payableAmount);
                    paidCommissionPercent += (selfCommissionPercent - paidCommissionPercent);
                }
                if (salesman.ReferenceCode != null)
                {
                    var reference = _salesman.Get(salesman.ReferenceCode).FirstOrDefault();
                    ProcessSalesCommission(con, tran, reference, salesAmount, paidCommissionPercent);
                }

        }
        private void ProcessGbCommission(SqlConnection con, SqlTransaction tran,SalesmanModel salesman)
        {
            var gbPolicy = _policy.GbCommissionPercentage();
            SalesmanModel reference = new SalesmanModel { Id = salesman.Id, Code = salesman.Code, ReferenceCode = salesman.Code };
            double salesAmount = 0, personalSalesAmount, commission;
            int level;
            for (level = 0; level < 10; level++)
            {
                if (reference == null) { break; }
                personalSalesAmount = _salesman.GetSalesAmount(code: reference.Code);
                if (_policy.IsEligible(personalSalesAmount))
                {
                    commission = salesAmount * gbPolicy[level] / 100;
                    _salary.AddGbCommission(connection: con, transaction: tran, reference.Code, commission);
                    reference = _salesman.Get(reference.Code).FirstOrDefault();
                }
                else
                {
                    if (level < 9)
                    {
                        gbPolicy[level + 1] += gbPolicy[level];
                    }
                }
            }
            while (level < 10)
            {
                commission = salesAmount * gbPolicy[level++] / 100;
                _salary.AddGbCommission(con, tran, "E-001", commission);
            }
        }
        private void ProcessOrdinalCommission(SqlConnection con, SqlTransaction tran)
        {
            var countByDesignation = _salesman.CountAssociates();
            double companiesSales = _member.GetSalesAmount(UserType.AlphaMember) +_member.GetSalesAmount(UserType.BetaMember);
            foreach(var dict in countByDesignation)
            {
                double commissionPercent = _policy.OrdinalCommissionPercentage(dict.Key);
                double commission = companiesSales * commissionPercent / 100;
                if (dict.Value>0)
                {
                    var salesmanList = _salesman.Get(dict.Key);
                    foreach(var salesman in salesmanList)
                    {
                        if (_policy.IsEligibleForOrdinalCommission(salesman))
                        {
                            _salary.AddOrdinalCommission(con,tran,salesman.Code,commission/dict.Value);
                        }
                    }
                }
                else
                {
                    _salary.AddOrdinalCommission(con, tran, "E-001", commission);
                }
            }
        }
    }
}
