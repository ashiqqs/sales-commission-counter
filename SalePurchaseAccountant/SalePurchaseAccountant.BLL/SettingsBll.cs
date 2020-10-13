using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.BasicSettings;
using SalePurchaseAccountant.Models.Helpers;
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
        public CompanyModel Registration(CompanyModel company)
        {
            if (GetCompany(company.Code) != null)
            {
                throw new InvalidException($"{company.Code} already exist.");
            }
            return _settingsDb.Registration(company);
        }
        public CompanyModel GetCompany(string code)
        {
            return _settingsDb.GetCompany(code);
        }
        public string GetNewCompanyCode()
        {
            return _settingsDb.GetNewCompanyCode();
        }
    }
}
