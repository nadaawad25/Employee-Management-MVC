using AutoMapper;
using Demo.PL.ViewModels;
using Deno.DAL.Models;

namespace Demo.PL.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile() {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
