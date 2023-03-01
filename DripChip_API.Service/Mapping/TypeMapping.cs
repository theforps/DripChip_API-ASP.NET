using AutoMapper;
using DripChip_API.Domain.DTO.Type;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Mapping;

public class TypeMapping : Profile
{
    public TypeMapping()
    {
        CreateMap<Types, DTOType>().ReverseMap();
        CreateMap<Types, DTOTypeInsert>().ReverseMap();
    }
}