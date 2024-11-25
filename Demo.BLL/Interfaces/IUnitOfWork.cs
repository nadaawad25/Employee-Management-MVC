using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Interfaces
{
    public interface IUnitOfWork
    {
        //Signiture For Prop foreach and every Repository 
        public IEmployeeRepository employeeRepository{ get; set; }
        public IDepartmentRepository departmentRepository { get; set; }

         Task<int> CompleteAsync();

        

    }
}
