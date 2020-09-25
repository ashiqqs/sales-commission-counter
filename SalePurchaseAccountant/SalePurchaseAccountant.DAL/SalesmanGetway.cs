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
    public class SalesmanGetway : IEmployee<SalesmanModel>,IAccount<SalesmanAccountModel>
    {
        public bool Add(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"INSERT INTO tblSalesman (Code, Name, ReferenceId,JoiningDate,CompanyId, ThanaId, Email, Address, ContactNo)
                                VALUES('{employee.Code}', '{employee.Name}',{employee.ReferenceId},'{employee.JoiningDate}',{employee.CompanyId},{employee.ThanaId},'{employee.Email}','{employee.Address}','{employee.ContactNo}')";
                return con.Execute(query) > 0;
            }
        }

        public bool Delete(int id)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"DELETE tblSalesman WHERE Id = {id}";
                return con.Execute(query) > 0;
            }
        }

        public List<SalesmanModel> Get(int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id==-1
                    ?$"SELECT * FROM tblSalesman"
                    :$"SELECT * FROM tblSalesman WHERE Id = {id}";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> Get(string code=null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = String.IsNullOrEmpty(code)
                    ? $"SELECT * FROM tblSalesman"
                    : $"SELECT * FROM tblSalesman WHERE Code = {code}";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }

        public bool Update(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UDATE tblSalesman SET Name='{employee.Name}', ReferenceId={employee.ReferenceId},JoiningDate='{employee.JoiningDate}'," 
                               +$"DesignationId={employee.Designaiton},CompanyId={employee.CompanyId},ThanaId={employee.ThanaId}, Email='{employee.Email}'," 
                               +$"Address='{employee.Address}', ContactNo='{employee.ContactNo}' WHERE Id = {employee.Id}";
                return con.Execute(query) > 0;
            }
        }
       
        public bool Purchase(SalesmanAccountModel salesmanAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblSalesmanAccounts (SalesmanId,Amount) VALUES ({salesmanAcc.SalesmanId},{salesmanAcc.Amount * -1})";
                return con.Execute(query) > 0;
            }
        }

        public bool Sale(SalesmanAccountModel salesmanAcc)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblSalesmanAccounts (SalesmanId,Amount) VALUES ({salesmanAcc.SalesmanId},{salesmanAcc.Amount})";
                return con.Execute(query)>0;
            }
        }

        public string GetNewCode(UserType type = UserType.SalesmanMan)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = "SELECT TOP 1 Code FROM tblSalesman ORDER BY Id DESC";
                string code = con.ExecuteScalar<string>(query) ?? "E-000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return "E-" + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }

        public int Count(int designation = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = designation == -1
                    ? "SELECT Count(Id) FROM tblSalesman"
                    : $"SELECT Count(Id) FROM tblSalesman WHERE Designation={designation}";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }

        public double GetSalesAmount(string month,UserType type=UserType.SalesmanMan, int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount) FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0 AND Id={id}"
                    :$"SELECT SUM(Amount) FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }

        public double GetPurchaseAmount(string month, UserType type = UserType.SalesmanMan, int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount)*-1 FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0 AND Id={id}"
                    :$"SELECT SUM(Amount)*-1 FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
    }
}
