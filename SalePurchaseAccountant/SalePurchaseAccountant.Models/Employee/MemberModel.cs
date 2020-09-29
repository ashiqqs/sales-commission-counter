using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Employee
{
    public class MemberModel:EmployeeModel
    {
        public int MemberType { get; set; }
        public string Sidc { get; set; }
    }
}
