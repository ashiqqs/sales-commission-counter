using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public class ThanaModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DistrictModel District { get; set; }
    }
}
