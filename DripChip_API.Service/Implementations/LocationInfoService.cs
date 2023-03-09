using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Enums;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class LocationInfoService:ILocationInfoService
{
    private readonly ILocationInfoRepository _locationInfoRepository;
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    public LocationInfoService(ILocationInfoRepository locationInfoRepository, IMapper mapper, IAnimalRepository animalRepository)
    {
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
}