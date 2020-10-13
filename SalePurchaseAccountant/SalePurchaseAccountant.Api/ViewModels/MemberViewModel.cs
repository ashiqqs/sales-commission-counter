using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Api.ViewModels
{
    public class MemberViewModel:EmployeeModel
    {
        public UserType MemberType { get; set; }
        public bool IsApproved { get; set; }
        public double TotalSales { get; set; }
        public double TotalPuchase { get; set; }
        public double InboundCommision { get; set; }
        public double OutBoundCommision { get; set; }
    }
}
