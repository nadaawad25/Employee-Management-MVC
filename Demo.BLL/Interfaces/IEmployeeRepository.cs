using Deno.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IEmployeeRepository :IGenericRepository<Employee>
    {
        IQueryable<Employee> GetEmplyeeByAddress(string address);
        IQueryable<Employee> GetEmplyeeByName(string searchTerm);
    }
}
