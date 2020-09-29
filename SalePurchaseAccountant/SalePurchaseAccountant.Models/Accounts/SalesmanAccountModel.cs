using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Accounts
{
    public class SalesmanAccountModel:AccountModel
    {
        public string MemberCode { get; set; }
        public string VendorCode { get; set; }
    }
}
