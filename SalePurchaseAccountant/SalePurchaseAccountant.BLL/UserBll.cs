using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class UserBll
    {
        private readonly UserGetway _userDb;
        public UserBll()
        {
            _userDb = new UserGetway();
        }
        public UserViewModel Login<UserViewModel>(string userName, string password)
        {
            var user = _userDb.Get<UserViewModel>(userName, password);
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
