using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface IEmployee<Employee>
    {
        string GetNewCode(string companyCode,UserType type);
        bool Add(Employee employee);
        bool Update(Employee employee);
        List<Employee> Get(string companyCode, string code=null);
        List<Employee> Get(string companyCode, int thanaId);
        bool Delete(int id);
        int Count(string companyCode, int memberType = -1);
    }
}
