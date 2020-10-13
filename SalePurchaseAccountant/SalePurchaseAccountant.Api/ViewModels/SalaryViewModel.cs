using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Api.ViewModels
{
    public class SalaryViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public double SalesCommission { get; set; }
        public double OrdinalCommission { get; set; }
        public double InboundCommission { get; set; }
        public double OutboundCommission { get; set; }
        public double GbCommission { get; set; }
        public string ProcessedMonth { get; set; }
        public string Total { get; set; }
    }
}
