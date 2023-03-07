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
    private readonly ITypeRepository _typeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IMapper _mapper;
    
    public AnimalService(IAnimalRepository animalRepository, IMapper mapper, ITypeRepository typeRepository, 
        IAccountRepository accountRepository, ILocationRepository locationRepository)
    {
        _mapper = mapper;
        _animalRepository = animalRepository;
        _typeRepository = typeRepository;
        _accountRepository = accountRepository;
        _locationRepository = locationRepository;
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

    public async Task<IBaseResponse<DTOAnimal>> AddAnimal(DTOAnimalAdd entity)
    {
        try
        {
            if (!entity.animalTypes.Any())
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Отсутсвуют типы животного",
                    StatusCode = StatusCode.Invalid
                };
            }
            
            foreach (var x in entity.animalTypes)
            {
                if (x <= 0)
                {
                    return new BaseResponse<DTOAnimal>()
                    {
                        Description = "Один или несколько типов животных <= 0",
                        StatusCode = StatusCode.Invalid
                    };
                }

                var checkType = await _typeRepository.GetTypeById(x);
                
                if (checkType == null)
                {
                    return new BaseResponse<DTOAnimal>()
                    {
                        Description = "Указан неизвестный тип животного",
                        StatusCode = StatusCode.NotFound
                    };
                }
            }

            var gender = new[] { "MALE", "FEMALE", "OTHER" };

            if (!gender.Contains(entity.gender))
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Неправильно указан гендер (MALE, FEMALE, OTHER)",
                    StatusCode = StatusCode.Invalid
                };
            }

            var checkChipper = await _accountRepository.GetUserById(entity.chipperId);

            if (checkChipper == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Неизвестный id пользователя, который чипировал животное",
                    StatusCode = StatusCode.NotFound
                };
            }

            var checkLocation = await _locationRepository.GetById(entity.chippingLocationId);
            
            if (checkLocation == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Неизвестный id локации",
                    StatusCode = StatusCode.NotFound
                };
            }
            
            var checkDuplicateType = entity.animalTypes;
            checkDuplicateType.Sort();
            for (int i = 1; i < checkDuplicateType.Count; i++)
            {
                if (checkDuplicateType[i] == checkDuplicateType[i - 1])
                {
                    return new BaseResponse<DTOAnimal>()
                    {
                        Description = "Дублирования типа животного",
                        StatusCode = StatusCode.Conflict
                    };
                }
            }

            var temp = _mapper.Map<DTOAnimal>(entity);
            temp.lifeStatus = "ALIVE";
            temp.chippingDateTime = DateTime.UtcNow;
            
            var animal = _mapper.Map<Animal>(temp);
            animal.animalTypes = await _typeRepository.GetTypesById(entity.animalTypes);
            
            var response = await _animalRepository.Add(animal);

            var result = _mapper.Map<DTOAnimal>(response);

            return new BaseResponse<DTOAnimal>()
            {
                StatusCode = StatusCode.OK,
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOAnimal>()
            {
                Description = $"AddAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
    public async Task<IBaseResponse<DTOAnimal>> UpdateAnimal(DTOAnimalUpdate entity, long id)
    {
        try
        {
            var checkChipper = await _accountRepository.GetUserById(entity.chepperId);

            if (checkChipper == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Пользователь не найден"
                };
            }

            var checkAnimal = await _animalRepository.GetById(id);

            if (checkAnimal == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Животное не найдено"
                };
            }

            var checkLocation = await _locationRepository.GetById(entity.chippingLocationId);

            if (checkLocation == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Точка локации не найдена"
                };
            }

            if (entity.lifeStatus == "ALIVE" && checkAnimal.lifeStatus == "DEAD")
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.Invalid,
                    Description = "Попытка оживить мертвое животное"
                };
            }

            if (entity.chippingLocationId == checkAnimal.visitedLocations[0].id)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.Invalid,
                    Description = "Новая точка чипирования совпадает с первой посещенной точкой локации"
                };
            }

            var animalForSetTimeDead = _mapper.Map<DTOAnimal>(entity);

            if (animalForSetTimeDead.lifeStatus == "DEAD" && checkAnimal.lifeStatus == "ALIVE")
            {
                animalForSetTimeDead.deathDateTime = DateTime.UtcNow;
            }

            var animalChanges = _mapper.Map<Animal>(animalForSetTimeDead);

            var animalUnity = _mapper.Map(checkAnimal,animalChanges);

            var response = await _animalRepository.Update(animalUnity);

            var result = _mapper.Map<DTOAnimal>(response);
            
            return new BaseResponse<DTOAnimal>()
            {
                StatusCode = StatusCode.OK,
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOAnimal>()
            {
                Description = $"UpdateAnimal : {ex.Message}",
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