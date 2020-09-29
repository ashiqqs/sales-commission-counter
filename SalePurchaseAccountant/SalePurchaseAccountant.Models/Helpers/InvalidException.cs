using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models.Helpers
{
    public class InvalidException : Exception
    {
        public InvalidException() : base() { }
        public InvalidException(string errMsg):base(errMsg){}
        public InvalidException(string errMsg,Exception innerException):base(errMsg,innerException){ }
    }
}
