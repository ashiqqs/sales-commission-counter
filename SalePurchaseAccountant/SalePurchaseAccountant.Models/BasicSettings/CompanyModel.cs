using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.BasicSettings
{
    public class CompanyModel
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
