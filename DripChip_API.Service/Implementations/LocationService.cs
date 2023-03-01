using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class LocationService : ILocationService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;

    public LocationService(ILocationRepository locationRepository, IMapper mapper)
    {
        _mapper = mapper;
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

    public async Task<IBaseResponse<Location>> AddLoc(DTOLocation entity)
    {
        try
        {
            var location = _mapper.Map<Location>(entity);

            var check = await _locationRepository.CheckExist(location);

            if (check)
            {
                return new BaseResponse<Location>()
                {
                    Description = "Локация уже существует",
                    StatusCode = StatusCode.LocationAlreadyExist
                };
            }

            var result = await _locationRepository.AddLocation(location);

            return new BaseResponse<Location>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Location>()
            {
                Description = $"AddLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<Location>> UpdateLoc(long id, DTOLocation entity)
    {
        try
        {
            var location = _mapper.Map<Location>(entity);

            var check = await _locationRepository.CheckExist(location);

            if (check)
            {
                return new BaseResponse<Location>()
                {
                    Description = "Локация уже существует",
                    StatusCode = StatusCode.LocationAlreadyExist
                };
            }

            location.id = id;

            var findLoc = await _locationRepository.GetById(id);

            if (findLoc == null)
            {
                return new BaseResponse<Location>()
                {
                    Description = "Локация не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }

            var result = await _locationRepository.UpdateLocation(location);

            return new BaseResponse<Location>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Location>()
            {
                Description = $"UpdateLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<bool>> DeleteLoc(long id)
    {
        try
        {
            var response = await _locationRepository.GetById(id);

            if (response == null)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Локация не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }

            var result = await _locationRepository.DeleteLocation(id);

            if (!result)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Локация связана с животным",
                    StatusCode = StatusCode.LocationRelated
                };
            }

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                Description = $"DeleteLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
}