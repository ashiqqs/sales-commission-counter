using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Employee
{
    public abstract class EmployeeModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ThanaId { get; set; }
        
        public DateTime JoiningDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
    }
}
