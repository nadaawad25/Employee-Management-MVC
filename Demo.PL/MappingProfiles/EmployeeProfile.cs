using AutoMapper;
using Demo.PL.ViewModels;
using Deno.DAL.Models;

namespace Demo.PL.MappingProfiles
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
               // .ForMember(d => d.Name, O => O.MapFrom(S => S.EmpName));

        }
    }
}
