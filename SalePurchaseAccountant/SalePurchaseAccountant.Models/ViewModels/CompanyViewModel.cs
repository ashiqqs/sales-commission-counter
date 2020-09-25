using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.ViewModels
{
    public class CompanyViewModel
    {
        public double TotalSales { get; set; }
        public double SalesCommission { get; set; }
        public double OrdinalCommission { get; set; }
    }
}
