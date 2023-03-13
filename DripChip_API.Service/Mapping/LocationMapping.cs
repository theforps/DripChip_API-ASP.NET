using AutoMapper;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Mapping;

public class LocationMapping:Profile
{
    public LocationMapping()
    {
        CreateMap<Location, DTOLocation>().ReverseMap();
        CreateMap<LocationInfo, DTOLocationInfo>()
            .ForMember(dest 
                => dest.locationPointId, opt 
                => opt.MapFrom(src => src.locationPoint.id));
        CreateMap<LocationInfo, LocationInfo>();
    }
}