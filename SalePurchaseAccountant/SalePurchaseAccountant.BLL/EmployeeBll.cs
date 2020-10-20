using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Accounts;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class EmployeeBll
    {
        private readonly IEmployee<SalesmanModel> _salesman;
        private readonly IAccount<SalesmanAccountModel> _salesmanAcc;
        private readonly IEmployee<MemberModel> _member;
        private readonly IAccount<MemberAccountModel> _memberAcc;
        public EmployeeBll()
        {
            _salesman = new SalesmanGetway();
            _salesmanAcc = new SalesmanGetway();
            _member = new MemberGetway();
            _memberAcc = new MemberGetway();
        }

        #region Salesman
        public string GetNewCode(string companyCode,UserType type)
        {
            switch (type)
            {
                case UserType.Salesman:
                    return _salesman.GetNewCode(companyCode,type);
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _member.GetNewCode(companyCode,type);
                default:
                    return null;
            }
        }
        public bool SaveOrUpdateSalesman(SalesmanModel salesman)
        {
            if (_salesman.Count(salesman.CompanyCode)>1 && String.IsNullOrEmpty(salesman.Code))
            {
                throw new InvalidException("SIDC is required.");
            }
            if(salesman.Id==null || salesman.Id == 0)
            {
                SalesmanModel existSalesman = _salesman.Get(salesman.CompanyCode,salesman.Code).FirstOrDefault();
                if (existSalesman == null)
                {
                    return _salesman.Add(salesman);
                }
                else
                {
                    throw new InvalidException($"{salesman.Code} already exist, try with another code.");
                }
            }
            else
            {
                return _salesman.Update(salesman);
            }
        }
        public List<SalesmanModel> GetSalesman(string companyCode,string code = null)
        {
            return _salesman.Get(companyCode,code);
        }
        public List<SalesmanModel> GetSalesmanByThana(string companyCode,int thanaId)
        {
            return _salesman.Get(companyCode,thanaId);
        }
        public bool DeleteSalesman(int id)
        {
            return _salesman.Delete(id);
        }
        public double PurchaseBySalesman(SalesmanAccountModel salesmanAcc)
        {
            if (_salesmanAcc.Purchase(salesmanAcc))
            {
                return _salesmanAcc.GetPurchaseAmount(salesmanAcc.CompanyCode, code: salesmanAcc.Code);
            }
            else
            {
                throw new InvalidException("Purchase Operation Failed.");
            }
        }
        public double SaleBySalesman(SalesmanAccountModel salesmanAcc)
        {
            if (_salesmanAcc.Sale(salesmanAcc)){
                return _salesmanAcc.GetSalesAmount(salesmanAcc.CompanyCode, code: salesmanAcc.Code);
            }
            else
            {
                throw new InvalidException("Sales Operation failed.");
            }
        }
        #endregion

        #region Member
        public bool SaveOrUpdateMember(MemberModel member)
        {
            if (String.IsNullOrEmpty(member.Code))
            {
                throw new InvalidException("Code is required.");
            }
            if (String.IsNullOrEmpty(member.Sidc))
            {
                throw new InvalidException("SIDC is required.");
            }
            if (member.Id == null || member.Id == 0)
            {
                if (member.MemberType==(int)UserType.AlphaMember && _member.Get(member.CompanyCode, member.ThanaId).Any(e => e.MemberType == (int)UserType.AlphaMember))
                {
                    throw new InvalidException("Alpha member already in selected thana");
                }
                if (GetMemberBySidc(member.CompanyCode, member.Sidc) != null)
                {
                    throw new InvalidException("Selected SIDC already have a member code");
                }
                MemberModel existMember = _member.Get(member.CompanyCode, member.Code).FirstOrDefault();
                if (existMember == null)
                {
                    return _member.Add(member);
                }
                else
                {
                    throw new InvalidException($"{member.Code} already exist, try with another code.");
                }
            }
            else
            {
                return _member.Update(member);
            }
        }
        public List<MemberModel> GetMember(string companyCode, string code = null)
        {
            return _member.Get(companyCode,code);
        }
        public MemberModel GetMemberBySidc(string companyCode,string sidc)
        {
            return new MemberGetway().GetMembershipInfo(companyCode,sidc);
        }
        public List<MemberModel> GetMemberByThana(string companyCode,int thanaId)
        {
            return _member.Get(companyCode,thanaId);
        }
        public bool DeleteMember(int id)
        {
            return _salesman.Delete(id);
        }
        public double PurchaseByMember(MemberAccountModel memberAcc)
        {
            if (_memberAcc.Purchase(memberAcc))
            {
                return _memberAcc.GetPurchaseAmount(memberAcc.CompanyCode, code: memberAcc.Code);
            }
            else
            {
                throw new InvalidException("Purchase Operation Failed.");
            }
        }
        public double SaleByMember(MemberAccountModel memberAcc)
        {
           if (_memberAcc.Sale(memberAcc))
            {
                return _memberAcc.GetSalesAmount(memberAcc.CompanyCode, memberAcc.UserType, code: memberAcc.Code);
            }
            else
            {
                throw new InvalidCastException("Sales Operation failed.");
            }
        }
        #endregion

        #region Employee
        public double PurchaseAmount(string companyCode, string month, UserType type, string code)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetPurchaseAmount(companyCode, type, month, code);
                case UserType.Salesman:
                    return _salesmanAcc.GetPurchaseAmount(companyCode, type, month, code);
                default:
                    return 0;
            }
        }
        public double SalesAmount(string companyCode, string month, UserType type, string code)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetSalesAmount(companyCode, type, month,  code);
                case UserType.Salesman:
                    return _salesmanAcc.GetSalesAmount(companyCode, type, month, code);
                default:
                    return _memberAcc.GetSalesAmount(companyCode, UserType.AlphaMember, month) + _memberAcc.GetSalesAmount(companyCode, UserType.BetaMember, month); ;
            }
        }
        public int Count(string companyCode, UserType type = UserType.Salesman)
        {
            if (type == UserType.Salesman)
            {
                return _salesman.Count(companyCode);
            }
            else
            {
                return _member.Count(companyCode,(int)type);
            }
        }
        public List<EmployeeViewModel> GetEmployees<EmployeeViewModel>(string companyCode)
        {
            return new SalesmanGetway().GetEmployees<EmployeeViewModel>(companyCode);
        }
        #endregion
    }
}
