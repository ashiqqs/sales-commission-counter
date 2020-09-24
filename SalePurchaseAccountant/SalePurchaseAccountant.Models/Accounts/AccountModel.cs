using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Accounts
{
    public abstract class AccountModel
    {
        public int Id { get; set; }
        public DateTime OperationDate { get; set; }
        public double Amount { get; set; }
    }
}
