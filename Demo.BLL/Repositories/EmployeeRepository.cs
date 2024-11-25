using Demo.BLL.Interfaces;
using Deno.DAL.Context;
using Deno.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private MvcDbContext _dbContext;
        public EmployeeRepository(MvcDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmplyeeByAddress(string address)
         => _dbContext.Employees.Where(e => e.Address == address);

        public IQueryable<Employee> GetEmplyeeByName(string SearchValue)
        => _dbContext.Employees.Include(e=> e.Department).Where(e => e.Name.ToLower().Contains(SearchValue.ToLower()) );



    }
}
