using Demo.BLL.Interfaces;
using Deno.DAL.Context;
using Deno.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department> ,IDepartmentRepository
    {
        private MvcDbContext _dbContext;
        public DepartmentRepository(MvcDbContext dbContext) : base(dbContext)
        {
           _dbContext = dbContext;
        }
    }
}
