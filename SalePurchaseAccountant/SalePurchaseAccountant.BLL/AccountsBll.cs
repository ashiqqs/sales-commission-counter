using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
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

        public bool ProcessSalary(string companyCode,string month)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                con.Open();
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        var topLevelSalesman = _salesman.GetAssociates(companyCode, companyCode);
                        _salary.CreateNewSalaryAcc(con, tran,companyCode,month);
                        ProcessOrdinalCommission(con, tran, companyCode);
                        ProcessSalesBoundGbCommission(con, tran, topLevelSalesman);
                        tran.Commit();
                        return true;
                    }
                    catch (Exception err)
                    {
                        tran.Rollback();
                        throw (err);
                    }
                }
            }
        }
        public List<SalaryViewModel> GetSalary<SalaryViewModel>(string companyCode, string code, string month)
        {
            return _salary.GetSalary<SalaryViewModel>(companyCode, code, month);
        }
        private void ProcessSalesBoundGbCommission(SqlConnection con, SqlTransaction tran, List<SalesmanModel> salesmanList)
        {
            foreach (var salesman in salesmanList)
            {
                double salesAmount = _salesman.GetSalesAmount(salesman.CompanyCode, code: salesman.Code);
                if (salesman.IsAlphaMember)
                {
                    double inboundCommissionPercent = _policy.InboundCommissionPercentage(salesAmount);
                    double inboundCommission = salesAmount * inboundCommissionPercent / 100;
                    _salary.AddInboundCommission(con, tran,salesman.CompanyCode, salesman.Code, inboundCommission);
                }
                if (salesman.IsBetaMember)
                {
                    double outboundCommissionPercent = _policy.OutboundCommissionPercentage(salesAmount);
                    double outboundCommission = salesAmount * outboundCommissionPercent / 100;
                    _salary.AddOutboundCommission(con, tran,salesman.CompanyCode, salesman.Code, outboundCommission);

                }
                ProcessSalesCommission(con, tran, salesman, salesAmount);
                ProcessGbCommission(con, tran, salesman, salesAmount);
                var associates = _salesman.GetAssociates(salesman.CompanyCode, salesman.Code);
                ProcessSalesBoundGbCommission(con, tran, associates);

                salesman.Designation = _policy.NextDesignation(salesman.CompanyCode, salesman.Code);
                _salesman.Update(con, tran, salesman);
            }
        }
        private void ProcessSalesCommission(SqlConnection con, SqlTransaction tran, SalesmanModel salesman, double salesAmount, double paidCommissionPercent = 0)
        {
            if (paidCommissionPercent == 26) { return; }

            double personalSalesAmount = _salesman.GetSalesAmount(salesman.CompanyCode,code: salesman.Code);
            double selfCommissionPercent = _policy.SalesCommissionPercentage(salesman.Designation, salesAmount);
            double payableAmount = salesAmount * (selfCommissionPercent - paidCommissionPercent) / 100;

            if (_policy.IsEligible(personalSalesAmount))
            {
                _salary.AddSalesCommission(con, tran, salesman.CompanyCode,salesman.Code, payableAmount);
                paidCommissionPercent += (selfCommissionPercent - paidCommissionPercent);
            }
            if (!string.IsNullOrEmpty(salesman.ReferenceCode))
            {
                var reference = _salesman.Get(salesman.CompanyCode,salesman.ReferenceCode).FirstOrDefault();
                if (reference == null)
                {
                    _salary.AddSalesCommission(con, tran, null, salesman.CompanyCode, salesAmount * (26 - paidCommissionPercent) / 100);
                }
                else
                {
                    ProcessSalesCommission(con, tran, reference, salesAmount, paidCommissionPercent);
                }
            }
        }
        private void ProcessGbCommission(SqlConnection con, SqlTransaction tran, SalesmanModel salesman, double salesAmount)
        {
            var gbPolicy = _policy.GbCommissionPercentage();
            SalesmanModel reference = new SalesmanModel { Id = salesman.Id, Code = salesman.Code, ReferenceCode = salesman.ReferenceCode, CompanyCode = salesman.CompanyCode };
            double personalSalesAmount, commission;
            int level;
            for (level = 0; level < 10; level++)
            {
                if (reference == null || string.IsNullOrEmpty(reference.ReferenceCode)) { break; }
                personalSalesAmount = _salesman.GetSalesAmount(reference.CompanyCode, code: reference.Code);
                if (_policy.IsEligible(personalSalesAmount))
                {
                    commission = salesAmount * gbPolicy[level] / 100;
                    _salary.AddGbCommission(connection: con, transaction: tran,reference.CompanyCode, reference.Code, commission);
                }
                else
                {
                    if (level < 9)
                    {
                        gbPolicy[level + 1] += gbPolicy[level];
                    }
                }

                reference = _salesman.Get(reference.CompanyCode, reference.ReferenceCode).FirstOrDefault();
                if (reference == null) { break; }
            }
            while (level < 10)
            {
                //Unpaid commisiion added to company account.
                commission = salesAmount * gbPolicy[level++] / 100;
                _salary.AddGbCommission(con, tran, null,salesman.CompanyCode, commission);
            }
        }
        private void ProcessOrdinalCommission(SqlConnection con, SqlTransaction tran, string companyCode)
        {
            var countByDesignation = _salesman.CountAssociates(companyCode);
            double companiesSales = _member.GetSalesAmount(companyCode,UserType.AlphaMember) + _member.GetSalesAmount(companyCode,UserType.BetaMember);
            foreach (var dict in countByDesignation)
            {
                double commissionPercent = _policy.OrdinalCommissionPercentage(dict.Key);
                double commission = companiesSales * commissionPercent / 100;

                var salesmanList = _salesman.Get(companyCode,dict.Key);
                int eligible = 0;
                foreach (var salesman in salesmanList)
                {
                    if (_policy.IsEligibleForOrdinalCommission(salesman))
                    {
                        _salary.AddOrdinalCommission(con, tran, companyCode,salesman.Code, commission / dict.Value);
                        eligible++;
                    }
                }
                if (eligible == 0)
                {
                    //Unpaid commisiion added to company account.
                    _salary.AddOrdinalCommission(con, tran,null, companyCode, commission);
                }
            }
        }
    }
}
