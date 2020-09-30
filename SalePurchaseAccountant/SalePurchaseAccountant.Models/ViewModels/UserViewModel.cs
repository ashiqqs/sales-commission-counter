using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.ViewModels
{
    public class UserViewModel:UserModel
    {
        public UserViewModel()
        {
            EmploymentInfo = new SalesmanViewModel();
        }
        public double SalesAmount { get; set; }
        public double PurchaseAmount { get; set; }
        public SalesmanViewModel EmploymentInfo { get; set; }
        
    }
}
