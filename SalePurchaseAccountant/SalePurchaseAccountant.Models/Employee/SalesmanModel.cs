using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Employee
{
    public class SalesmanModel:EmployeeModel
    {
        public int ReferenceId { get; set; }
        public Designation Designaiton { get; set; }
        public bool IsAlphaMember { get; set; }
        public bool IsBetaMember { get; set; }

    }
}
