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
        List<Employee> Get(int id=-1);
        List<Employee> Get(string code=null);
        bool Delete(int id);
        int Count(int memberType = -1);
    }
}
