using Dapper;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.Accounts;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class SalesmanGetway : IEmployee<SalesmanModel>, IAccount<SalesmanAccountModel>
    {
        public bool Add(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"INSERT INTO tblSalesman (Code, Name, ReferenceCode,JoiningDate, ThanaId, Email, Address, ContactNo,CompanyCode)
                                VALUES('{employee.Code}', '{employee.Name}','{employee.ReferenceCode}','{employee.JoiningDate.Date}',{employee.ThanaId},'{employee.Email}','{employee.Address}','{employee.ContactNo}','{employee.CompanyCode}')";
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
        public List<SalesmanModel> Get(string companyCode,int thanaId)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman WHERE ThanaId={thanaId} AND CompanyCode='{companyCode}'";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> Get(string companyCode,string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (string.IsNullOrEmpty(code) || code == "null")
                    ? $"SELECT * FROM tblSalesman WHERE IsAlphaMember=0 AND IsBetaMember=0 AND CompanyCode='{companyCode}'"
                    : $"SELECT * FROM tblSalesman WHERE Code = '{code}'  AND CompanyCode='{companyCode}'";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> Get(string companyCode,Designation designation)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman WHERE Designation = {(int)designation} AND CompanyCode='{companyCode}'";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> GetAssociates(string companyCode,string referenceCode, int designation = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = designation == -1
                    ? $"SELECT * FROM tblSalesman WHERE ReferenceCode='{referenceCode}' AND CompanyCode='{companyCode}' "
                    : $"SELECT * FROM tblSalesman WHERE ReferenceCode='{referenceCode}' AND Designation={(int)designation} AND CompanyCode='{companyCode}'";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public bool Update(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UPDATE tblSalesman SET Name='{employee.Name}', ReferenceCode={employee.ReferenceCode},JoiningDate='{employee.JoiningDate.Date}',"
                               + $"Designation={(int)employee.Designation},ThanaId={employee.ThanaId}, Email='{employee.Email}',"
                               + $"Address='{employee.Address}', ContactNo='{employee.ContactNo}' WHERE Id = {employee.Id}";
                return con.Execute(query) > 0;
            }
        }
        public void Update(SqlConnection con, SqlTransaction tran, SalesmanModel employee)
        {
            string query = $@"UPDATE tblSalesman SET Name='{employee.Name}', ReferenceCode='{employee.ReferenceCode}',JoiningDate='{employee.JoiningDate.Date}',"
                                 + $"Designation={(int)employee.Designation},ThanaId={employee.ThanaId}, Email='{employee.Email}',"
                                 + $"Address='{employee.Address}', ContactNo='{employee.ContactNo}' WHERE Code = '{employee.Code}' AND CompanyCode='{employee.CompanyCode}'";
            con.Execute(query, transaction: tran);

        }

        public bool Purchase(SalesmanAccountModel salesmanAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount, CompanyCode) " +
                           $"VALUES ('{salesmanAcc.Code}',{salesmanAcc.Amount * -1}, '{salesmanAcc.CompanyCode}')," +
                           $"('{salesmanAcc.VendorCode}',{salesmanAcc.Amount},'{salesmanAcc.CompanyCode}')";
                return con.Execute(query) > 0;
            }
        }
        public bool Sale(SalesmanAccountModel salesmanAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount, CompanyCode) VALUES ('{salesmanAcc.Code}',{salesmanAcc.Amount},'{salesmanAcc.CompanyCode}')";
                return con.Execute(query) > 0;
            }
        }

        public string GetNewCode(string companyCode,UserType type = UserType.Salesman)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT TOP 1 Code FROM tblSalesman WHERE CompanyCode='{companyCode}' ORDER BY Id DESC";
                string code = con.ExecuteScalar<string>(query) ?? "E-000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return "E-" + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }
        public int Count(string companyCode,int designation = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = designation == -1
                    ? $"SELECT Count(Code) FROM tblSalesman WHERE CompanyCode='{companyCode}'"
                    : $"SELECT Count(Code) FROM tblSalesman WHERE WHERE CompanyCode='{companyCode}' AND Designation={designation}";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }
        public Dictionary<Designation, int> CountAssociates(string companyCode,string code = null)
        {
            Dictionary<Designation, int> associates = new Dictionary<Designation, int>();

            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (string.IsNullOrEmpty(code) || code == "null")
                        ? $"SELECT COUNT(Code) FROM tblSalesman WHERE CompanyCode='{companyCode}' AND Designation="
                        : $"SELECT COUNT(Code) FROM tblSalesman WHERE CompanyCode='{companyCode}' AND ReferenceCode='{code}' AND Designation=";

                for (int i = 1; i <= 11; i++)
                {
                    int count = con.ExecuteScalar<int>(query + i);
                    associates.Add((Designation)i, count);
                }
                return associates;
            }
        }

        public double GetSalesAmount(string companyCode,UserType type = UserType.Salesman, string month = null, string code = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (string.IsNullOrEmpty(code) || code == "null")
                    ? $"SELECT SUM(Amount) FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0 AND CompanyCode='{companyCode}'"
                    : $"SELECT SUM(Amount) FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0 AND CompanyCode='{companyCode}' AND Code='{code}'";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double GetAssociatesSalesAmount(string companyCode,string referenceCode, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query;
            using (var con = ConnectionGetway.GetConnection())
            {
                query = $"SELECT SUM(Amount) FROM tblAccounts a JOIN tblSalesman sm ON a.Code=sm.Code WHERE a.CompanyCode='{companyCode}' AND CONVERT(VARCHAR(6),a.OperationDate,112)='{month}' AND a.Amount>0 AND sm.ReferenceCode='{referenceCode}'";
                double associatesAmount = con.ExecuteScalar<double>(query);
                return associatesAmount;
            }
        }
        public double GetPurchaseAmount(string companyCode,UserType type = UserType.Salesman, string month = null, string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (string.IsNullOrEmpty(code) || code == "null")
                    ? $"SELECT SUM(Amount)*-1 FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0 AND CompanyCode='{companyCode}'"
                    : $"SELECT SUM(Amount)*-1 FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0 AND Code='{code}' AND CompanyCode='{companyCode}'";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }

        public List<EmployeeViewModel> GetEmployees<EmployeeViewModel>(string companyCode)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                var employees = con.Query<EmployeeViewModel>($"SELECT * FROM vw_employees WHERE CompanyCode='{companyCode}'").ToList();
                return employees;
            }
        }
    }
}
