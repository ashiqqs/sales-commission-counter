using Dapper;
using SalePurchaseAccountant.Models.Accounts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class SalaryGetway
    {
        public bool CreateNewSalaryAcc(string month=null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                using (var tran = con.BeginTransaction())
                {
                    try
                    {
                        month = month ?? DateTime.Now.ToString("yyyyMM");
                        int rowAffect = con.Execute("usp_CreateSalaryAcc", param:new { SalaryMonth=month}, transaction:tran, commandType:CommandType.StoredProcedure);
                        tran.Commit();
                        return rowAffect > 0;
                    }
                    catch (Exception err)
                    {
                        tran.Rollback();
                        throw new Exception(err.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="amount"></param>
        /// <param name="month">provide in yyyyMM format</param>
        /// <returns></returns>
        public bool AddSalesCommission(string code, double amount, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = $"UPDATE tblSalaries SET SalesCommission = SalesCommission+{amount} WHERE Code={code} AND ProcessedMonth='{month}'";
                return con.Execute(query) > 0;
            }
        }
        public bool AddOrdinalCommission(string code, double amount, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = $"UPDATE tblSalaries SET OrdinalCommission = OrdinalCommission+{amount} WHERE Code={code} AND ProcessedMonth='{month}'";
                return con.Execute(query) > 0;
            }
        }
        public bool AddInboundCommission(string code, double amount, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = $"UPDATE tblSalaries SET InboundCommission = InboundCommission+{amount} WHERE Code={code} AND ProcessedMonth='{month}'";
                return con.Execute(query) > 0;
            }
        }
        public bool AddOutboundCommission(string code, double amount, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = $"UPDATE tblSalaries SET OutboundCommission = OutboundCommission+{amount} WHERE Code={code} AND ProcessedMonth='{month}'";
                return con.Execute(query) > 0;
            }
        }
        public bool AddGbCommission(string code, double amount, string month = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = $"UPDATE tblSalaries SET GbCommission = GbCommission+{amount} WHERE Code={code} AND ProcessedMonth='{month}'";
                return con.Execute(query) > 0;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">Default is null for getting salary of any code</param>
        /// <param name="month">Default is null for getting salary of any current month</param>
        /// <returns></returns>
        public List<SalaryModel> GetSalary(string code=null, string month = null)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                month = month ?? DateTime.Now.ToString("yyyyMM");
                string query = String.IsNullOrEmpty(code)
                    ? $"SELECT * FROM tblSalaries WHERE ProcessedMonth = '{month}'"
                    : $"SELECt * FROM tblSalaries WHERE ProcessedMonth='{month}' AND Code='{code}'";
                var salaries = con.Query<SalaryModel>(query).ToList();
                return salaries;
            }
        }
    }
}
