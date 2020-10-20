using Dapper;
using SalePurchaseAccountant.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class SalaryGetway
    {
        public void CreateNewSalaryAcc(SqlConnection connection, SqlTransaction transaction, string companyCode, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            connection.Execute("usp_CreateSalaryAcc", param: new { SalaryMonth = month, CompanyCode=companyCode }, transaction: transaction, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="amount"></param>
        /// <param name="month">value should be in yyyyMM format</param>
        /// <returns></returns>
        public void AddSalesCommission(SqlConnection connection, SqlTransaction transaction, string companyCode, string code, double amount, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query =companyCode!=null
                ?$"UPDATE tblSalaries SET SalesCommission = SalesCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}' AND CompanyCode='{companyCode}'"
                :$"UPDATE tblSalaries SET SalesCommission = SalesCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}'";
            connection.Execute(query, transaction: transaction);

        }
        public void AddOrdinalCommission(SqlConnection connection, SqlTransaction transaction, string companyCode, string code, double amount, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query = companyCode!=null
                ?$"UPDATE tblSalaries SET OrdinalCommission = OrdinalCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}' AND CompanyCode='{companyCode}'"
                :$"UPDATE tblSalaries SET OrdinalCommission = OrdinalCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}'";
            connection.Execute(query, transaction: transaction);
        }
        public void AddInboundCommission(SqlConnection connection, SqlTransaction transaction, string companyCode,string code, double amount, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query = companyCode!=null
                ?$"UPDATE tblSalaries SET InboundCommission = InboundCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}' AND CompanyCode='{companyCode}'"
                :$"UPDATE tblSalaries SET InboundCommission = InboundCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}'";
            connection.Execute(query, transaction: transaction);
        }
        public void AddOutboundCommission(SqlConnection connection, SqlTransaction transaction, string companyCode, string code, double amount, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query = companyCode != null
                ? $"UPDATE tblSalaries SET OutboundCommission = OutboundCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}' AND CompanyCode='{companyCode}'"
                : $"UPDATE tblSalaries SET OutboundCommission = OutboundCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}'";
            connection.Execute(query, transaction: transaction);
        }
        public void AddGbCommission(SqlConnection connection, SqlTransaction transaction, string companyCode, string code, double amount, string month = null)
        {
            month = month ?? DateTime.Now.ToString("yyyyMM");
            string query = companyCode!=null?
                $"UPDATE tblSalaries SET GbCommission = GbCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}' AND CompanyCode='{companyCode}'"
                :$"UPDATE tblSalaries SET GbCommission = GbCommission+{amount} WHERE Code='{code}' AND ProcessedMonth='{month}'";
            connection.Execute(query, transaction: transaction);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">Default is null for getting salary of any code</param>
        /// <param name="month">Default is null for getting salary of current month</param>
        /// <returns></returns>
        public List<SalaryViewModel> GetSalary<SalaryViewModel>(string companyCode,string code = null, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = (string.IsNullOrEmpty(month) || month=="null") ? DateTime.Now.ToString("yyyyMM"):month;
                object paramObj = new
                {
                    ProcessedMonth = month,
                    Code = (string.IsNullOrEmpty(code) || code=="null")?"":code??"",
                    CompanyCode = companyCode == "null" ? "" : companyCode ?? ""
                };
                var salaries = con.Query<SalaryViewModel>("usp_GetSalary",param:paramObj, commandType:CommandType.StoredProcedure).ToList();
                return salaries;
            }
        }
    }
}
