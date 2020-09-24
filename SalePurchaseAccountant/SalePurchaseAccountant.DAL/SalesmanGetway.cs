using Dapper;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Employee;
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
            employee.Code = GetNewCode();
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

        public List<SalesmanModel> Get()
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }

        public SalesmanModel Get(int id)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman WHERE Id = {id}";
                return con.QuerySingle<SalesmanModel>(query);
            }
        }
       
        public bool Update(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UDATE tblSalesman 
SET Code='{employee.Code}', Name='{employee.Name}', ReferenceId={employee.ReferenceId},JoiningDate='{employee.JoiningDate}', 
DesignationId={employee.DesignaitonId},CompanyId={employee.CompanyId},ThanaId={employee.ThanaId}, Email='{employee.Email}', 
Address='{employee.Address}', ContactNo='{employee.ContactNo}' WHERE Id = {employee.Id}";
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

        public string GetNewCode()
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = "SELECT TOP 1 Code FROM tblSalesman ORDER BY Id DESC";
                string code = con.ExecuteScalar<string>(query) ?? "E-000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return "E-" + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }

        public int Count(int designationId = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = designationId == -1
                    ? "SELECT Count(Id) FROM tblSalesman"
                    : $"SELECT Count(Id) FROM tblSalesman WHERE DesignationId={designationId}";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }

        public double GetSalesAmount(DateTime fromDate, DateTime toDate,int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount) FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount>0 AND Id={id}"
                    :$"SELECT SUM(Amount) FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount>0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }

        public double GetPurchaseAmount(DateTime fromDate, DateTime toDate,int id=-1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = id!=-1
                    ?$"SELECT SUM(Amount)*-1 FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount<0 AND Id={id}"
                    :$"SELECT SUM(Amount)*-1 FROM tblSalesmanAccounts WHERE CONVERT(VARCHAR,OperationDate,112)>={fromDate.ToString("yyyyMMdd")} AND CONVERT(VARCHAR,OperationDate,112)<={toDate.ToString("yyyyMMdd")} AND Amount<0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
    }
}
