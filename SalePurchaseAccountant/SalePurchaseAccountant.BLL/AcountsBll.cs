using SalePurchaseAccountant.Models.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class AcountsBll
    {
        private readonly PolicyBll _policy;
        public AcountsBll()
        {
            _policy = new PolicyBll();
        }
        
    }
}
