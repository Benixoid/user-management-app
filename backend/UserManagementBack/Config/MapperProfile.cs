using AutoMapper;
using UserManagementBack.Models;
using UserManagementBack.Models.DTO;

namespace UserManagementBack.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }        
    }
}
