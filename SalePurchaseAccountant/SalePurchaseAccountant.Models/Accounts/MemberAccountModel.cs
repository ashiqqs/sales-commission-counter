using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Accounts
{
    public class MemberAccountModel:AccountModel
    {
        public string Sidc { get; set; }
        public string CustomerCode { get; set; }
    }
}
