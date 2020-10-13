using SalePurchaseAccountant.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Api.ViewModels
{
    public class SalesmanViewModel
    {
        public SalesmanViewModel()
        {
            Membership = new MemberModel();
        }
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ThanaId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public int DesignationId { get; set; }
        public bool IsAlphaMember { get; set; }
        public bool IsBetaMember { get; set; }
        public MemberModel Membership { get; set; }
        public SalesmanViewModel Reference { get; set; }
        public List<SalesmanViewModel> Associates { get; set; }
        public double TotalSales { get; set; }
        public double TotalPurchase { get; set; }
        public double SalesCommision { get; set; }
        public double OrdinalCommision { get; set; }
        public double InboundCommision { get; set; }
        public double OutBoundCommision { get; set; }
        public double GbCommision { get; set; }
    }
}
