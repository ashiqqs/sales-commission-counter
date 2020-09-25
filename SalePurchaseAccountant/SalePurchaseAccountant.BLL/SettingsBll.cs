using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.BasicSettings;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class SettingsBll
    {
        private readonly SettingsGetway _settingsDb;
        public SettingsBll()
        {
            _settingsDb = new SettingsGetway();
        }
        public List<DistrictModel> GetDistrict(int districtId)
        {
            return _settingsDb.GetDistrict(districtId);
        }
        public List<ThanaModel> GetThana(int districtId,int thanaId)
        {
            return _settingsDb.GetThana(districtId,thanaId);
        }
        public string GetNewCompanyCode()
        {
            return _settingsDb.GetNewCompanyCode();
        }
        public bool SaveCompany(CompanyModel company)
        {
            if ((company.Id==null || company.Id==0) && _settingsDb.Get(company.Code).Count > 0)
            {
                throw new Exception($"{company.Code} already exist, try with another code.");
            }
            return _settingsDb.SaveCompany(company);
        }
    }
}
