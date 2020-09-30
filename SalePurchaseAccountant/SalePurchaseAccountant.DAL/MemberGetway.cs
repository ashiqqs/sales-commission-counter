using Dapper;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Accounts;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class MemberGetway : IEmployee<MemberModel>, IAccount<MemberAccountModel>
    {
        public bool Add(MemberModel member)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"INSERT INTO tblMembers (Code, Name, MemberType,JoiningDate, ThanaId, Email, Address, ContactNo,Sidc)
                                VALUES('{member.Code}', '{member.Name}',{member.MemberType},'{member.JoiningDate.Date}',{member.ThanaId},'{member.Email}','{member.Address}','{member.ContactNo}','{member.Sidc}')";
                return con.Execute(query) > 0;
            }
        }
        public bool Delete(string code)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"DELETE tblMembers WHERE Code = '{code}'";
                return con.Execute(query) > 0;
            }
        }
        public List<MemberModel> Get()
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblMembers";
                return con.Query<MemberModel>(query).ToList();
            }
        }
        public List<MemberModel> Get(int thanaId)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblMembers WHERE ThanaId={thanaId}";
                return con.Query<MemberModel>(query).ToList();
            }
        }
        public List<MemberModel> Get(string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (string.IsNullOrEmpty(code) || code == "null")
                    ? $"SELECT * FROM tblMembers"
                    : $"SELECT * FROM tblMembers WHERE code = '{code}'";
                return con.Query<MemberModel>(query).ToList();
            }
        }
        public bool Update(MemberModel member)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UPDATE tblMembers SET Name='{member.Name}', MemberType={member.MemberType},JoiningDate='{member.JoiningDate}', 
 Email='{member.Email}',Address='{member.Address}', ContactNo='{member.ContactNo}' WHERE Code = {member.Code}";
                return con.Execute(query) > 0;
            }
        }

        public bool Purchase(MemberAccountModel memberAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount) VALUES ('{memberAcc.Sidc}',{memberAcc.Amount * -1})";
                return con.Execute(query) > 0;
            }
        }
        public bool Sale(MemberAccountModel memberAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount)" +
                    $"VALUES ('{memberAcc.Sidc}',{memberAcc.Amount})," +
                    $"('{memberAcc.CustomerCode}',{memberAcc.Amount*-1})";
                return con.Execute(query) > 0;
            }
        }

        public string GetNewCode(UserType type)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT TOP 1 Code FROM tblMembers WHERE MemberType={(int)type} ORDER BY Id DESC";
                string prefix = type == UserType.AlphaMember ? "AL-"
                    : (type == UserType.BetaMember) ? "B-"
                    : throw new Exception("Invalid Member Type");
                string code = con.ExecuteScalar<string>(query) ?? prefix + "000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return prefix + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }

        public int Count(int memberType = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = memberType != -1
                    ? $"SELECT Count(Code) FROM tblMembers WHERE MemberType={(int)memberType}"
                    : "SELECT Count(Code) FROM tblMembers";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }

        public double GetSalesAmount(UserType type, string month = null, string code = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (!string.IsNullOrEmpty(code) && code != "null")
                    ?((type==UserType.AlphaMember)? $"SELECT SUM(a.Amount) FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE a.Code={code} AND sm.IsAlphaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount>0"
                     :$"SELECT SUM(a.Amount) FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE a.Code={code} AND sm.IsBetaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount>0")
                    :( (type == UserType.AlphaMember)
                    ? $"SELECT SUM(a.Amount) FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE sm.IsAlphaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount>0"
                    : $"SELECT SUM(a.Amount) FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE sm.IsBetaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount>0 ");
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double GetPurchaseAmount(UserType type, string month = null, string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("YYYYMM");
                string query = (!string.IsNullOrEmpty(code) && code != "null")
                    ? ((type == UserType.AlphaMember) ? $"SELECT SUM(a.Amount)*-1 FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE a.Code={code} AND sm.IsAlphaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount<0"
                     : $"SELECT SUM(a.Amount)*-1 FROM tblAccounts a  JOIN tblSalesman sm ON a.Code=sm.Code WHERE a.Code={code} AND sm.IsBetaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount<0")
                    : ((type == UserType.AlphaMember)
                    ? $"SELECT SUM(a.Amount)*-1 FROM tblAccounts a  JOIN tblSalesman sm ON a.Code=sm.Code WHERE sm.IsAlphaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount<0"
                    : $"SELECT SUM(a.Amount)*-1 FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE sm.IsBetaMember=1 AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount<0");
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public MemberModel GetMembershipInfo(string sidc)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblMembers WHERE Sidc='{sidc}' ";
                return con.Query<MemberModel>(query).FirstOrDefault();
            }
        }
    }
}
