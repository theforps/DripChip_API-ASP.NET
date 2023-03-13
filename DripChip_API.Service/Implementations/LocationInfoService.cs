using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class LocationInfoService:ILocationInfoService
{
    private readonly ILocationInfoRepository _locationInfoRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    public LocationInfoService(ILocationInfoRepository locationInfoRepository,
        IMapper mapper, IAnimalRepository animalRepository, ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
        _mapper = mapper;
        _locationInfoRepository = locationInfoRepository;
        _animalRepository = animalRepository;
    }
    
    public async Task<IBaseResponse<List<DTOLocationInfo>>> GetLocationStory(long id, DTOAnimalSearchLocation animal)
    {
        try
        {
            var checkAnimal = await _animalRepository.GetById(id);

            if (checkAnimal == null)
            {
                return new BaseResponse<List<DTOLocationInfo>>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            
            var response = _locationInfoRepository.GetAnimalLocations(
                id,
                animal.from,
                animal.size,
                DateTime.Parse(animal.startDateTime),
                DateTime.Parse(animal.endDateTime));

            var locationStory = _mapper.Map<List<DTOLocationInfo>>(response);
            
            if (!locationStory.Any() || locationStory == null)
            {
                return new BaseResponse<List<DTOLocationInfo>>()
                {
                    Description = "Посещенные локации не найдены",
                    StatusCode = StatusCode.LocationStoryNotFound
                };
            }
            
            return new BaseResponse<List<DTOLocationInfo>>()
            {
                StatusCode = StatusCode.OK,
                Data = locationStory,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<DTOLocationInfo>>()
            {
                Description = $"GetLocationStory : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
    public async Task<IBaseResponse<DTOLocationInfo>> AddVisitedLocation(long animalId, long pointId)
    {
        try
        {
            var checkAnimal = await _animalRepository.GetById(animalId);
            var checkLocation = await _locationRepository.GetById(pointId);

            if (checkAnimal == null)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            else if (checkLocation == null)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Локация не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }
            else if (checkAnimal.lifeStatus == "DEAD")
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное мертво",
                    StatusCode = StatusCode.AnimalIsDead
                };
            }
            else if (!checkAnimal.visitedLocations.Any() && checkAnimal.chippingLocationId == pointId)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное никуда не перемещалось и точка чипирования = посещенной точке",
                    StatusCode = StatusCode.Invalid
                };
            }
            if (checkAnimal.visitedLocations.Select(x => x.locationPoint.id == pointId) == 
                checkAnimal.visitedLocations.LastOrDefault())
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное уже находится в этой точке",
                    StatusCode = StatusCode.Invalid
                };
            }

            var locationInfo = new LocationInfo
            {
                locationPoint = checkLocation,
                animal = checkAnimal
            };

            var responseAddLocToAnimal = await _locationInfoRepository.AddToAnimal(animalId, locationInfo);

            var result = _mapper.Map<DTOLocationInfo>(responseAddLocToAnimal);

            return new BaseResponse<DTOLocationInfo>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOLocationInfo>()
            {
                Description = $"AddVisitedLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
    public async Task<IBaseResponse<DTOLocationInfo>> EditVisitedLocation(long animalId, DTOLocationInfoEdit entity)
    {
        try
        {
            var checkAnimal = await _animalRepository.GetById(animalId);
            var checkLocation = await _locationRepository.GetById(entity.locationPointId);
            var checkLocationInfo = await _locationInfoRepository.Get(entity.visitedLocationPointId);
            var locationInfoList = await _locationInfoRepository.GetList(animalId);
            
            if (checkAnimal == null)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            else if (checkLocation == null)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Локация не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }            
            else if (checkLocationInfo == null)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Информация о посещенной точке локации не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }
            else if (!checkAnimal.visitedLocations.Any(x => x.id == checkLocationInfo.id))
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное не содержит данную посещенную точку локации",
                    StatusCode = StatusCode.LocationNotFound
                };
            }
            else if (checkAnimal.visitedLocations.First().locationPoint == checkLocation && entity.visitedLocationPointId == 0)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Животное не содержит данную посещенную точку локации",
                    StatusCode = StatusCode.LocationNotFound
                };
            }
            else if (checkAnimal.visitedLocations[0].id == entity.visitedLocationPointId &&
                     entity.locationPointId == checkAnimal.chippingLocationId)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Обновление первой посещенной точки на точку чипирования",
                    StatusCode = StatusCode.Invalid
                };
            }
            else if (checkLocationInfo.locationPoint.id == entity.locationPointId &&
                     checkLocationInfo.id == entity.visitedLocationPointId)
            {
                return new BaseResponse<DTOLocationInfo>()
                {
                    Description = "Обновление точки на такую же точку",
                    StatusCode = StatusCode.Invalid
                };
            }
            else if (locationInfoList != null && locationInfoList.Count() > 1)
            {
                for (int i = 1; i < locationInfoList.Count(); i++)
                {
                    if (locationInfoList[i].id == entity.visitedLocationPointId)
                    {
                        if (locationInfoList[i - 1].locationPoint.id == entity.locationPointId)
                        {
                            return new BaseResponse<DTOLocationInfo>()
                            {
                                Description =
                                    "Обновление точки локации на точку, совпадающую со следующей и/или с предыдущей точками",
                                StatusCode = StatusCode.Invalid
                            };
                        }
                        else if (locationInfoList.Count() != i + 1 &&
                                 locationInfoList[i + 1].locationPoint.id == entity.locationPointId)
                        {
                            return new BaseResponse<DTOLocationInfo>()
                            {
                                Description =
                                    "Обновление точки локации на точку, совпадающую со следующей и/или с предыдущей точками",
                                StatusCode = StatusCode.Invalid
                            };
                        }
                    }
                }
            }

            var locationInfo = _mapper.Map<LocationInfo>(checkLocationInfo);
            locationInfo.locationPoint = checkLocation;

            var responseUpdateLocToAnimal = await _locationInfoRepository.Update(locationInfo);

            var result = _mapper.Map<DTOLocationInfo>(responseUpdateLocToAnimal);

            return new BaseResponse<DTOLocationInfo>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOLocationInfo>()
            {
                Description = $"DeleteVisitedLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<bool>> DeleteVisitedLocation(long animalId, long visitedPointId)
    {
        try
        {
            var checkAnimal = await _animalRepository.GetById(animalId);
            var checkLocationInfo = await _locationInfoRepository.Get(visitedPointId);
            
            if (checkAnimal == null)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            else if (checkLocationInfo == null)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Информация о посещенной точке локации не найдена",
                    StatusCode = StatusCode.LocationNotFound
                };
            }
            else if (!checkAnimal.visitedLocations.Any(x => x.id == checkLocationInfo.id))
            {
                return new BaseResponse<bool>()
                {
                    Description = "Животное не содержит данную посещенную точку локации",
                    StatusCode = StatusCode.LocationNotFound
                };
            }

            var response = await _locationInfoRepository.Delete(animalId, visitedPointId);

            return new BaseResponse<bool>()
            {
                StatusCode = StatusCode.OK
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                Description = $"DeleteVisitedLocation : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
}