using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalePurchaseAccountant.Api.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Type { get; set; }
        public DateTime JoiningDate { get; set; }
        public string CompanyCode { get; set; }
    }
}
