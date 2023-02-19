using AutoMapper;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.DTO.Type;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Mapping;

public class AnimalMapping : Profile
{
    public AnimalMapping()
    {
        CreateMap<Animal, DTOAnimalSearch>().ReverseMap();
        CreateMap<Animal, LocationInfo>().ReverseMap();
        CreateMap<Types, DTOType>().ReverseMap();
        CreateMap<LocationInfo, DTOLocationInfo>().ReverseMap();
    }
}