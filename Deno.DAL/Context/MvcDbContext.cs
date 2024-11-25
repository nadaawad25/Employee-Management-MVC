using Deno.DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deno.DAL.Context
{
    public class MvcDbContext :IdentityDbContext<ApplicationUser>

    {
        public MvcDbContext(DbContextOptions<MvcDbContext> options):base(options)
        {
            
        }
        //1.create on Cofiguring and useSqlServer
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // => optionsBuilder.UseSqlServer("Server =.; Database=MvcApp;Trusted_Connection = true;");
       
        //2.create Dbset
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        //comment it as we inherit IdentityDbContext

        //public DbSet<IdentityUser> Users { get; set; }

        //public DbSet<IdentityRole> Roles { get; set; }



        //3.make migration 


    }
}
