using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface IEmployee<Employee>
    {
        string GetNewCode(UserType type);
        bool Add(Employee employee);
        bool Update(Employee employee);
        List<Employee> Get(string code=null);
        List<Employee> Get(int thanaId);
        bool Delete(string code);
        int Count(int memberType = -1);
    }
}
