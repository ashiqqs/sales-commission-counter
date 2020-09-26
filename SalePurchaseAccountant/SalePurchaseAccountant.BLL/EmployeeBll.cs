using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Accounts;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using SalePurchaseAccountant.Models.ViewModels;
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
            if(salesman.Id==null || salesman.Id == 0)
            {
                SalesmanModel existSalesman = _salesman.Get(salesman.Code).FirstOrDefault();
                if (existSalesman == null)
                {
                    return _salesman.Add(salesman);
                }
                else
                {
                    throw new Exception($"{salesman.Code} already exist, try with another code.");
                }
            }
            else
            {
                return _salesman.Update(salesman);
            }
        }
        public List<SalesmanModel> GetSalesman(int id=-1)
        {
            return _salesman.Get(id);
        }
        public List<SalesmanModel> GetSalesman(string code = null)
        {
            return _salesman.Get(code);
        }
        public bool DeleteSalesman(int id)
        {
            return _salesman.Delete(id);
        }
        public bool PurchaseBySalesman(SalesmanAccountModel salesmanAcc)
        {
            return _salesmanAcc.Purchase(salesmanAcc);
        }
        public bool SaleBySalesman(SalesmanAccountModel salesmanAcc)
        {
            return _salesmanAcc.Sale(salesmanAcc);
        }
        #endregion

        #region Member
        public bool SaveOrUpdateMember(MemberModel member)
        {
            if (member.Id == null || member.Id == 0)
            {
                MemberModel existSalesman = _member.Get(member.Code).FirstOrDefault();
                if (existSalesman == null)
                {
                    return _member.Add(member);
                }
                else
                {
                    throw new Exception($"{member.Code} already exist, try with another code.");
                }
            }
            else
            {
                return _member.Update(member);
            }
        }
        public List<MemberModel> GetMember(int id = -1)
        {
            return _member.Get(id);
        }
        public List<MemberModel> GetMember(string code = null)
        {
            return _member.Get(code);
        }
        public bool DeleteMember(int id)
        {
            return _salesman.Delete(id);
        }
        public bool PurchaseByMember(MemberAccountModel memberAcc)
        {
            return _memberAcc.Purchase(memberAcc);
        }
        public bool SaleByMember(MemberAccountModel memberAcc)
        {
            return _memberAcc.Sale(memberAcc);
        }
        #endregion

        #region Employee
        public double PurchaseAmount(string month, UserType type, int id=-1)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetPurchaseAmount(month, type, id);
                case UserType.Salesman:
                    return _salesmanAcc.GetPurchaseAmount(month, type, id);
                default:
                    return 0;
            }
        }
        public double SalesAmount(string month, UserType type, int id = -1)
        {
            switch (type)
            {
                case UserType.AlphaMember:
                case UserType.BetaMember:
                    return _memberAcc.GetSalesAmount(type, month,  id);
                case UserType.Salesman:
                    return _salesmanAcc.GetSalesAmount(type, month, id);
                default:
                    return 0;
            }
        }
        #endregion
    }
}
