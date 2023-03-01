using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IMapper _mapper;
    
    public AnimalService(IAnimalRepository animalRepository, IMapper mapper)
    {
        _mapper = mapper;
        _animalRepository = animalRepository;
    }

    #region Animal
    
    public async Task<IBaseResponse<DTOAnimal>> GetAnimal(long id)
    {
        try
        {
            var response = await _animalRepository.GetById(id);

            var animal = _mapper.Map<DTOAnimal>(response);

            if (animal == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            
            return new BaseResponse<DTOAnimal>()
            {
                StatusCode = StatusCode.OK,
                Data = animal
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOAnimal>()
            {
                Description = $"GetAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
        
            
    }
    
    public async Task<IBaseResponse<List<DTOAnimal>>> GetAnimalByParam(DTOAnimalSearch animal)
    {
        try
        {
            var search = _mapper.Map<Animal>(animal);
            
            var response = _animalRepository.GetByParams(search,animal.from,animal.size, 
                DateTime.Parse(animal.startDateTime),DateTime.Parse(animal.endDateTime));

            var animals = _mapper.Map<List<DTOAnimal>>(response);
            
            if (!animals.Any())
            {
                return new BaseResponse<List<DTOAnimal>>()
                {
                    Description = "Животные не найдены",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }

            return new BaseResponse<List<DTOAnimal>>()
            {
                StatusCode = StatusCode.OK,
                Data = animals,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<DTOAnimal>>()
            {
                Description = $"GetAnimalsByParam : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
    
    #endregion

    #region Location

    public async Task<IBaseResponse<List<DTOLocationInfo>>> GetLocationStory(long id, DTOAnimalSearchLocation animal)
    {
        try
        {
            var response = _animalRepository.GetAnimalLocations(
                id,
                animal.from,
                animal.size,
                DateTime.Parse(animal.startDateTime),
                DateTime.Parse(animal.endDateTime));

            var locationStory = _mapper.Map<List<DTOLocationInfo>>(response);
            
            if (!locationStory.Any())
            {
                return new BaseResponse<List<DTOLocationInfo>>()
                {
                    Description = "Локации с такими параметрами не найдены",
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

    #endregion
}