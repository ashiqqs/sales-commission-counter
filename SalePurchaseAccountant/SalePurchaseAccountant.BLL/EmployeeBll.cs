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
        public string GetNewCode(UserType type)
        {
            switch (type)
            {
                case UserType.Salesman:
                    return _salesman.GetNewCode(type);
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _member.GetNewCode(type);
                default:
                    return null;
            }
        }
        public bool SaveOrUpdateSalesman(SalesmanModel salesman)
        {
            if (_salesman.Count()>1 && String.IsNullOrEmpty(salesman.Code))
            {
                throw new InvalidException("SIDC is required.");
            }
            if(salesman.Id==null || salesman.Id == 0)
            {
                SalesmanModel existSalesman = _salesman.Get(salesman.Code).FirstOrDefault();
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
        public List<SalesmanModel> GetSalesman(string code = null)
        {
            return _salesman.Get(code);
        }
        public List<SalesmanModel> GetSalesmanByThana(int thanaId)
        {
            return _salesman.Get(thanaId);
        }
        public bool DeleteSalesman(string code)
        {
            return _salesman.Delete(code);
        }
        public double PurchaseBySalesman(SalesmanAccountModel salesmanAcc)
        {
            if (_salesmanAcc.Purchase(salesmanAcc))
            {
                return _salesmanAcc.GetPurchaseAmount(code: salesmanAcc.Code);
            }
            else
            {
                throw new InvalidException("Purchase Operation Failed.");
            }
        }
        public double SaleBySalesman(SalesmanAccountModel salesmanAcc)
        {
            if (_salesmanAcc.Sale(salesmanAcc)){
                return _salesmanAcc.GetSalesAmount(code: salesmanAcc.Code);
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
                if (member.MemberType==(int)UserType.AlphaMember && _member.Get(member.ThanaId).Any(e => e.MemberType == (int)UserType.AlphaMember))
                {
                    throw new InvalidException("Alpha member already in selected thana");
                }
                if (GetMemberBySidc(member.Sidc) != null)
                {
                    throw new InvalidException("Selected SIDC already have a member code");
                }
                MemberModel existMember = _member.Get(member.Code).FirstOrDefault();
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
        public List<MemberModel> GetMember(string code = null)
        {
            return _member.Get(code);
        }
        public MemberModel GetMemberBySidc(string sidc)
        {
            return new MemberGetway().GetMembershipInfo(sidc);
        }
        public List<MemberModel> GetMemberByThana(int thanaId)
        {
            return _member.Get(thanaId);
        }
        public bool DeleteMember(string code)
        {
            return _salesman.Delete(code);
        }
        public double PurchaseByMember(MemberAccountModel memberAcc)
        {
            if (_memberAcc.Purchase(memberAcc))
            {
                return _memberAcc.GetPurchaseAmount(code: memberAcc.Code);
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
                return _memberAcc.GetSalesAmount(memberAcc.UserType, code: memberAcc.Code);
            }
            else
            {
                throw new InvalidCastException("Sales Operation failed.");
            }
        }
        #endregion

        #region Employee
        public double PurchaseAmount(string month, UserType type, string code)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetPurchaseAmount(type, month, code);
                case UserType.Salesman:
                    return _salesmanAcc.GetPurchaseAmount(type, month, code);
                default:
                    return 0;
            }
        }
        public double SalesAmount(string month, UserType type, string code)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetSalesAmount(type, month,  code);
                case UserType.Salesman:
                    return _salesmanAcc.GetSalesAmount(type, month, code);
                default:
                    return _memberAcc.GetSalesAmount(UserType.AlphaMember, month) + _memberAcc.GetSalesAmount(UserType.BetaMember, month); ;
            }
        }
        public int Count(UserType type = UserType.Salesman)
        {
            if (type == UserType.Salesman)
            {
                return _salesman.Count();
            }
            else
            {
                return _member.Count((int)type);
            }
        }
        #endregion
    }
}
