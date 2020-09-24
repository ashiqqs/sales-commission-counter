using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface ICommission
    {
        double GetSalesCommission(int id);
        double GetOrdinalCommission(int id);
        double GetInboundCommission(int id);
        double GetOutboundCommission(int id);
        double GetGbCommission(int id);
    }
}
