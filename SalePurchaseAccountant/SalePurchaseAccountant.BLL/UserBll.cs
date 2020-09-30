using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using SalePurchaseAccountant.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class UserBll
    {
        private readonly UserGetway _userDb;
        private readonly EmployeeBll _employee;
        public UserBll()
        {
            _userDb = new UserGetway();
            _employee = new EmployeeBll();
        }
        public UserViewModel Login(string userName, string password)
        {
            UserViewModel user = _userDb.Get(userName, password);
            if (user==null ||( user.Name != userName && user.Password != password))
            {
                throw new InvalidException("Incorrect username or password!");
            }
            user.PurchaseAmount = _employee.PurchaseAmount(DateTime.Now.ToString("yyyyMM"), user.UserType, user.Code);
            user.SalesAmount = _employee.SalesAmount(DateTime.Now.ToString("yyyyMM"), user.UserType, user.Code);
            if (user.UserType == UserType.AlphaMember || user.UserType == UserType.BetaMember)
            {
                user.EmploymentInfo.Membership = _employee.GetMember(user.Code).FirstOrDefault() ?? new MemberModel();
            }
            else
            {
                SalesmanModel emp = _employee.GetSalesman(user.Code).FirstOrDefault();
                user.EmploymentInfo.Id = emp.Id;
                user.EmploymentInfo.Code = emp.Code;
                user.EmploymentInfo.Name = emp.Name;
                user.EmploymentInfo.IsAlphaMember = emp.IsAlphaMember;
                user.EmploymentInfo.IsBetaMember = emp.IsBetaMember;
                user.EmploymentInfo.Membership = _employee.GetMemberBySidc(user.Code) ?? new MemberModel();
            }
            return user;
        }
        public bool ChangePassword(UserModel user)
        {
            UserModel existUser = _userDb.Get((int)user.Id);
            if (existUser != null && existUser.Password == user.OldPassword)
            {
                return _userDb.ChangePassword(user);
            }
            else
            {
                return false;
            }

        }
    }
}
