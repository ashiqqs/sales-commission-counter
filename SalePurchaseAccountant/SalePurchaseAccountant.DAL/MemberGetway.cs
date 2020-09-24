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
            member.Code = GetNewCode((EmployeeType)member.MemberType);
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
        public MemberModel Get(int id)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblMembers WHERE Id = {id}";
                return con.QuerySingle<MemberModel>(query);
            }
        }
        public bool Update(MemberModel member)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UDATE tblMembers SET Code='{member.Code}', Name='{member.Name}', MemberType={member.MemberType},JoiningDate='{member.JoiningDate}', 
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

        public string GetNewCode(EmployeeType type)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT TOP 1 Code FROM tblMembers WHERE MemberType={type} ORDER BY Id DESC";
                string prefix = type == EmployeeType.AlphaMember ? "AL-"
                    : (type == EmployeeType.BetaMember) ? "B-"
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

        public double GetSalesAmount(DateTime fromDate, DateTime toDate,int id=-1)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount) FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount>0 AND Id={id}"
                    :$"SELECT SUM(Amount) FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount>0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double GetSalesAmount(DateTime fromDate, DateTime toDate, EmployeeType type)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT SUM(Amount) FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount>0 AND MemberType={type}";
                    double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }

        public double GetPurchaseAmount(DateTime fromDate, DateTime toDate,int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount)*-1 FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount<0 AND Id={id}"
                    :$"SELECT SUM(Amount)*-1 FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount<0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double PurchaseAmount(DateTime fromDate, DateTime toDate, EmployeeType type)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT SUM(Amount)*-1 FROM tblMemberAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount<0 AND MemberType={type}";
                    double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
    }
}
