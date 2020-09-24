using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface IAccount<Account>
    {
        bool Sale(Account account);
        double GetSalesAmount(DateTime fromDate, DateTime toDate,int id=-1);
        bool Purchase(Account account);
        double GetPurchaseAmount(DateTime fromDate, DateTime toDate,int id=-1);
    }
}
