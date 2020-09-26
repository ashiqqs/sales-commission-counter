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
    public class MemberGetway : IEmployee<MemberModel>,IAccount<MemberAccountModel>
    {
        public bool Add(MemberModel member)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"INSERT INTO tblMembers (Code, Name, MemberType,JoiningDate,CompanyId, ThanaId, Email, Address, ContactNo,IsApproved)
                                VALUES('{member.Code}', '{member.Name}',{member.MemberType},'{member.JoiningDate}',{member.CompanyId},{member.ThanaId},'{member.Email}','{member.Address}','{member.ContactNo}',{member.IsApproved})";
                return con.Execute(query) > 0;
            }
        }
        public bool Delete(int id)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"DELETE tblMembers WHERE Id = {id}";
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
        public List<MemberModel> Get(int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id==-1
                    ?$"SELECT * FROM tblMembers"
                    :$"SELECT * FROM tblMembers WHERE Id = {id}";
                return con.Query<MemberModel>(query).ToList();
            }
        }
        public List<MemberModel> Get(string code=null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = String.IsNullOrEmpty(code)
                    ?$"SELECT * FROM tblMembers"
                    :$"SELECT * FROM tblMembers WHERE Code = {code}";
                return con.Query<MemberModel>(query).ToList();
            }
        }
        public bool Update(MemberModel member)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UDATE tblMembers SET Name='{member.Name}', MemberType={member.MemberType},JoiningDate='{member.JoiningDate}', 
CompanyId={member.CompanyId},ThanaId={member.ThanaId}, Email='{member.Email}', 
Address='{member.Address}', ContactNo='{member.ContactNo}',IsApproved = {member.IsApproved} WHERE Id = {member.Id}";
                return con.Execute(query) > 0;
            }
        }

        public bool Purchase(MemberAccountModel memberAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblMemberAccounts (SalesmanId,Amount) VALUES ({memberAcc.MemberId},{memberAcc.Amount*-1})";
                return con.Execute(query) > 0;
            }
        }
        public bool Sale(MemberAccountModel memberAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblMemberAccounts (SalesmanId,Amount) VALUES ({memberAcc.MemberId},{memberAcc.Amount})";
                return con.Execute(query) > 0;
            }
        }

        public string GetNewCode(UserType type)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT TOP 1 Code FROM tblMembers WHERE MemberType={type} ORDER BY Id DESC";
                string prefix = type == UserType.AlphaMember ? "AL-"
                    : (type == UserType.BetaMember) ? "B-"
                    : throw new Exception("Invalid Member Type");
                string code = con.ExecuteScalar<string>(query) ?? prefix + "000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return prefix + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }
        
        public int Count(int memberType=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = memberType!=-1
                    ?$"SELECT Count(Id) FROM tblMembers WHERE MemberType={memberType}"
                    : "SELECT Count(Id) FROM tblMembers";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }

        public double GetSalesAmount( UserType type, string month = null, int id=-1)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id != -1
                    ? $"SELECT SUM(Amount) FROM tblMemberAccounts WHERE Id={id} AND MemberType={type} AND CONVERT(VARCHAR(6),OperationDate,112)='{month}'"
                    : $"SELECT SUM(Amount) FROM tblMemberAccounts WHERE MemberType={type} AND CONVERT(VARCHAR(6),OperationDate,112)='{month}'";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double GetPurchaseAmount(string month, UserType type, int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id != -1
                    ? $"SELECT SUM(Amount)*-1 FROM tblMemberAccounts WHERE  AND Id={id} AND MemberType={type} AND CONVERT(VARCHAR(6),OperationDate,112)='{month}'"
                    : $"SELECT SUM(Amount)*-1 FROM tblMemberAccounts WHERE MemberType={type} AND CONVERT(VARCHAR(6),OperationDate,112)='{month}'";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
}
}
