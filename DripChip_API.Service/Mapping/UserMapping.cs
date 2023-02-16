using AutoMapper;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, DTOUserRegistration>().ReverseMap();
        CreateMap<User, DTOUser>().ReverseMap();
        CreateMap<User, DTOUserSearch>().ReverseMap();
    }
}