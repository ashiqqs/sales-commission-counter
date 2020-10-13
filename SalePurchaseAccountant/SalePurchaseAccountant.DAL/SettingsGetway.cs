using Dapper;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.BasicSettings;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalePurchaseAccountant.DAL
{
    public class SettingsGetway
    {
        public List<DistrictModel> GetDistrict(int districtId = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                var districts = con.Query<DistrictModel>($"Select * FROM tblDistricts WHERE Id = CASE WHEN {districtId}<>-1 THEN {districtId} ELSE Id END").ToList();
                return districts;
            }
        }
        public List<ThanaModel> GetThana(int districtId = -1, int thanaId = -1)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                var thana = con.Query<ThanaModel>($@"Select * FROM tblThana WHERE DistrictId =  (CASE WHEN {districtId}<>-1 THEN {districtId} ELSE DistrictId END) AND Id = (CASE WHEN {thanaId}<>-1 THEN {thanaId} ELSE Id END) ").ToList();
                return thana;
            }
        }
        public CompanyModel Registration(CompanyModel company)
        {
            using(var con = ConnectionGetway.GetConnection())
            {
                string query = $"INSERT INTO tblCompanies(Code, Name,Address) VALUES('{company.Code}', '{company.Name}',{company.Address}";
                int rowAffect = con.Execute(query);
                if (rowAffect > 0) { return company; }
                else { return null; }
            }
        }
        public CompanyModel GetCompany(string code)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = $"SELECt * FROM tblCompanies WHERE Code = '{code}'";
                var company = con.Query<CompanyModel>(query).FirstOrDefault();
                return company;
            }
        }
        public string GetNewCompanyCode()
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = "SELECT TOP 1 Code FROM tblCompanies ORDER BY Id DESC";
                string code = con.ExecuteScalar<string>(query) ?? "C-000";
                int.TryParse(code.Split('-')[1], out int codeNum);
                return "C-" + ((codeNum + 1).ToString().PadLeft(3, '0'));
            }
        }
    }
}
