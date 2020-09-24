using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.Models
{
    public interface IEmployee<Employee>
    {
        bool Add(Employee employee);
        bool Update(Employee employee);
        List<Employee> Get();
        Employee Get(int id);
        bool Delete(int id);
        int Count(int memberType = -1);
    }
}
