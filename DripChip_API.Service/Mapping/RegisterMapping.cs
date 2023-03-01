using AutoMapper;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Mapping;

public class RegisterMapping : Profile
{
    public RegisterMapping()
    {
        CreateMap<User, DTOUserRegistration>().ReverseMap();
        CreateMap<User, DTOUser>().ReverseMap();
    }
}