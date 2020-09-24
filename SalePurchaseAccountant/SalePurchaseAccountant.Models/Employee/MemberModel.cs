using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Employee
{
    public class MemberModel:EmployeeModel
    {
        public int MemberType { get; set; }
        public bool IsApproved { get; set; }
    }
}
