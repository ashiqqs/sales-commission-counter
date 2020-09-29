using Dapper;
using SalePurchaseAccountant.Models;
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
        
    }
}
