using AutoMapper;
using CleanArch.Application.DTOs.Customer;
using CleanArch.Application.DTOs.User;
using CleanArch.Application.Features.Customer.Queries.GetAllCustomers;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Entities.Identity;

namespace CleanArch.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User

            CreateMap<ApplicationUser, ApplicationUserDto>();
            CreateMap<ApplicationRole, ApplicationRoleDto>();
            CreateMap<ApplicationUserRole, ApplicationUserRoleDto>();

            #endregion

            #region Customer

            CreateMap<Customer, ListCustomerDto>().ReverseMap();

            #endregion
        }
    }
}
