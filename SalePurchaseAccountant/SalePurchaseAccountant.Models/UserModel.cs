using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public class UserModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
        public string Code { get; set; }
        public string CompanyCode { get; set; }
    }
}
