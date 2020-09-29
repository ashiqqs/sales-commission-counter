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
    public class SalesmanGetway : IEmployee<SalesmanModel>, IAccount<SalesmanAccountModel>
    {
        public bool Add(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"INSERT INTO tblSalesman (Code, Name, ReferenceCode,JoiningDate, ThanaId, Email, Address, ContactNo)
                                VALUES('{employee.Code}', '{employee.Name}','{employee.ReferenceCode}','{employee.JoiningDate.Date}',{employee.ThanaId},'{employee.Email}','{employee.Address}','{employee.ContactNo}')";
                return con.Execute(query) > 0;
            }
        }
        public bool Delete(string code)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"DELETE tblSalesman WHERE Code = '{code}'";
                return con.Execute(query) > 0;
            }
        }
        public List<SalesmanModel> Get(int thanaId)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman WHERE ThanaId={thanaId}";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> Get(string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = code == null
                    ? $"SELECT * FROM tblSalesman"
                    : $"SELECT * FROM tblSalesman WHERE Code = '{code}'";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> Get(Designation designation)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECT * FROM tblSalesman WHERE Designation = {designation}";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public List<SalesmanModel> GetAssociates(string referenceCode, int designation = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = designation == -1
                    ? $"SELECT * FROM tblSalesman WHERE ReferenceCode={referenceCode}"
                    : $"SELECT * FROM tblSalesman WHERE ReferenceCode={referenceCode} AND Designation={designation}";
                return con.Query<SalesmanModel>(query).ToList();
            }
        }
        public bool Update(SalesmanModel employee)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $@"UDATE tblSalesman SET Name='{employee.Name}', ReferenceCode={employee.ReferenceCode},JoiningDate='{employee.JoiningDate.Date}',"
                               + $"Designation={employee.Designaiton},ThanaId={employee.ThanaId}, Email='{employee.Email}',"
                               + $"Address='{employee.Address}', ContactNo='{employee.ContactNo}' WHERE Id = {employee.Id}";
                return con.Execute(query) > 0;
            }
        }

        public bool Purchase(SalesmanAccountModel salesmanAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount) " +
                           $"VALUES ({salesmanAcc.Code},{salesmanAcc.Amount * -1})," +
                           $" VALUES ({salesmanAcc.VendorCode},{salesmanAcc.Amount * 1})";
                return con.Execute(query) > 0;
            }
        }
        public bool Sale(SalesmanAccountModel salesmanAcc)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblAccounts (Code,Amount) VALUES ({salesmanAcc.Code},{salesmanAcc.Amount})";
                return con.Execute(query) > 0;
            }
        }

        public string GetNewCode(UserType type = UserType.Salesman)
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
                    ? "SELECT Count(Code) FROM tblSalesman"
                    : $"SELECT Count(Code) FROM tblSalesman WHERE Designation={designation}";
                int count = con.ExecuteScalar<int>(query);
                return count;
            }
        }
        public Dictionary<Designation, int> CountAssociates(string code = null)
        {
            Dictionary<Designation, int> associates = new Dictionary<Designation, int>();

            using (var con = ConnectionGetway.GetConnection())
            {
                string query = code == null
                        ? $"SELECT COUNT(Code) FROM tblSalesman WHERE Designation="
                        : $"SELECT COUNT(Code) FROM tblSalesman WHERE ReferenceCode={code} AND Designation=";

                for (int i = 1; i <= 11; i++)
                {
                    int count = con.ExecuteScalar<int>(query + i);
                    associates.Add((Designation)i, count);
                }
                return associates;
            }
        }

        public double GetSalesAmount(UserType type = UserType.Salesman, string month = null, string code = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = code == null
                    ? $"SELECT SUM(Amount) FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0 AND Code={code}"
                    : $"SELECT SUM(Amount) FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }
        public double GetAssociatesSalesAmount(string referenceCode, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query;
            using (var con = ConnectionGetway.GetConnection())
            {
                query = $"SELECT SUM(Amount) FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount>0 AND ReferenceCode={referenceCode}";
                double associatesAmount = con.ExecuteScalar<double>(query);
                return associatesAmount;
            }
        }
        public double GetPurchaseAmount(UserType type = UserType.Salesman, string month = null, string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = code == null
                    ? $"SELECT SUM(Amount)*-1 FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0 AND Code={code}"
                    : $"SELECT SUM(Amount)*-1 FROM tblAccounts WHERE CONVERT(VARCHAR(6),OperationDate,112)='{month}' AND Amount<0";
                double amount = con.ExecuteScalar<double>(query);
                return amount;
            }
        }

    }
}
