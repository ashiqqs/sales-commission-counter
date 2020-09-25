using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface ICommission
    {
        double GetSalesCommission(int id, string month=null);
        double GetOrdinalCommission(int id, string month=null);
        double GetInboundCommission(int id, string month = null);
        double GetOutboundCommission(int id, string month = null);
        double GetGbCommission(int id, string month = null);
    }
}
