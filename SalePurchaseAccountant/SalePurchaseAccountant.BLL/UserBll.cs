using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using System;
using System.Collections.Generic;
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
        public UserModel Login(string userName, string passowrd)
        {
            UserModel user = _userDb.Get(userName, passowrd);
            if (user == null)
            {
                throw new Exception("Incorrect username or password!");
            }
            return user;
        }
        public bool ChangePassword(UserModel user)
        {
            UserModel existUser = _userDb.Get(user.Id);
            if (existUser!=null && existUser.Password == user.OldPassword)
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
