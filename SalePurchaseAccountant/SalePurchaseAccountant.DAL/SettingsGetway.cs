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
        public bool SaveCompany(CompanyModel company)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = (company.Id == null || company.Id == 0)
        ? $@"INSERT INTO Companies (Name, Address, Email,Website,ContactNo, Code) VALUES('{company.Name}', '{company.Address}', '{company.Email}', '{company.Website}', '{company.ContactNo}','{company.Code}')"
        : $@"UPDATE Companies SET Name='{company.Name}', Address='{company.Address}', Email='{company.Email}',Website='{company.Website}', ContactNo='{company.ContactNo}' WHERE Id={company.Id}";

                return con.Execute(query) > 0;
            }
        }
        public List<CompanyModel> Get(string code = null)
        {
            using (var con = ConnectionGetway.GetConnection())
            {
                string query = String.IsNullOrEmpty(code)
                    ? $"SELECT * FROM tblCompanies"
                    : $"SELECT * FROM tblCompanies WHERE Code='{code}'";
                return con.Query<CompanyModel>(query).ToList();
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
