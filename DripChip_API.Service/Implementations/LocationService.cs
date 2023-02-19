using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;

    public LocationService(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }
    
    public async Task<IBaseResponse<Location>> GetById(long id)
    {
        try
        {
            var location = await _locationRepository.GetById(id);

            if (location == null)
            {
                return new BaseResponse<Location>()
                {
                    Description = "Локация не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }

            return new BaseResponse<Location>()
            {
                StatusCode = StatusCode.OK,
                Data = location,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Location>()
            {
                Description = $"GetLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
}