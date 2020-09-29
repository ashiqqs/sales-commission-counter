using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface IAccount<Account>
    {
        bool Sale(Account account);
        bool Purchase(Account account);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month">month should be yyyyMM format</param>
        /// <param name="type">Default user type assigned Salesman</param>
        /// <param name="id"></param>
        /// <returns></returns>
        double GetSalesAmount(UserType type = UserType.Salesman, string month=null, string code=null);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="month">month should be yyyyMM format</param>
        /// <param name="type">Default user type assigned Salesman</param>
        /// <param name="id"></param>
        /// <returns></returns>
        double GetPurchaseAmount(UserType type = UserType.Salesman, string month=null, string code =null);
    }
}
