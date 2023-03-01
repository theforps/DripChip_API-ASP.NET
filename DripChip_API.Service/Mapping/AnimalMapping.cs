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
        CreateMap<Animal, DTOAnimal>()
            .ForMember(dest 
                => dest.visitedLocations, opt 
                => opt.MapFrom(src => src.visitedLocations.Select(y => y.id).ToList()))
            .ForMember(dest 
                => dest.animalTypes, opt 
                => opt.MapFrom(src => src.animalTypes.Select(y => y.id).ToList()));
    }
}