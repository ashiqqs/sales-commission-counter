using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.BasicSettings
{
    public class CompanyModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string Website { get; set; }
    }
}
