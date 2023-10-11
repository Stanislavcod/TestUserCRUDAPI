using AutoMapper;
using UserTestCRUD.BusinessLogic.ViewModel;
using UserTestCRUD.DAL.Entities;

namespace UserTestCRUD.BusinessLogic.Mapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Roles, opt => 
                opt.MapFrom(src => src.Roles.Select(r => new RoleViewModel { Name = r.Name })));

            CreateMap<UserViewModel, User>()
                .ForMember(dest => dest.Roles, opt => 
                opt.MapFrom(src => src.Roles.Select(roleViewModel 
                => new Role { Name = roleViewModel.Name })));
        }
    }
}
