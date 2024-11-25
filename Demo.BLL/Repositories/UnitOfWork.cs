using Demo.BLL.Interfaces;
using Deno.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{//implement interface 
    public class UnitOfWork : IUnitOfWork ,IDisposable
    {
        private MvcDbContext _dbContext;
        public IEmployeeRepository employeeRepository { get ; set; }
        public IDepartmentRepository departmentRepository { get ; set; }
        public UnitOfWork(MvcDbContext dbContext)
        {
            employeeRepository = new EmployeeRepository(dbContext);
            departmentRepository = new DepartmentRepository(dbContext);
            _dbContext = dbContext;
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Dispose of the dbContext
            _dbContext.Dispose();
        }
    }
}
