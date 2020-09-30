using Dapper;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class UserGetway
    {
        public bool ChangePassword(UserModel user)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"UPDATE tblUsers SET Password = '{user.Password}' WHERE Code = '{user.Code}'";
                return con.Execute(query)>0;
            }
        }
        public UserViewModel Get(string userName, string password)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblUsers WHERE Name='{userName}' AND Password='{password}'";
                return con.Query<UserViewModel>(query).FirstOrDefault();
            }
        }
        public UserModel Get(int id)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblUsers WHERE Id={id}";
                return con.Query<UserModel>(query).FirstOrDefault();
            }
        }
    }
}
