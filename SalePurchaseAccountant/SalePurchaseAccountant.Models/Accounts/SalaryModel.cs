using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Accounts
{
    public class SalaryModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int UserType { get; set; }
        public double SalesCommission { get; set; }
        public double OrdinalCommission { get; set; }
        public double InboundCommission { get; set; }
        public double OutboundCommission { get; set; }
        public double GbCommission { get; set; }
        public string ProcessedMonth { get; set; }
    }
}
