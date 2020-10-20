using SalePurchaseAccountant.Models;
using SalePurchaseAccountant.Models.BasicSettings;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Api.ViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            EmploymentInfo = new SalesmanViewModel();
        }
        public int? Id { get; set; }
        public string Name { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Code { get; set; }
        public double SalesAmount { get; set; }
        public double PurchaseAmount { get; set; }
        public SalesmanViewModel EmploymentInfo { get; set; }
        public string CompanyCode { get; set; }
        public CompanyModel Company { get; set; }

    }
}
