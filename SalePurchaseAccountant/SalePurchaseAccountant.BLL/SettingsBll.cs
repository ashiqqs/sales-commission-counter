using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models;
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
    }
}
