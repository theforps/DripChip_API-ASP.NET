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
    
    public async Task<IBaseResponse<DTOAnimal>> GetAnimal(long id)
    {
        try
        {
            var response = await _animalRepository.GetById(id);

            if (response == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            
            var animal = _mapper.Map<DTOAnimal>(response);
            
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
            
            if (entity.animalTypes.Count > 1)
            {
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
            }

            var animal = _mapper.Map<Animal>(entity);
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
            var checkChipper = await _accountRepository.GetUserById(entity.chipperId);

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
            else if (entity.lifeStatus == "DEAD" && checkAnimal.lifeStatus == "ALIVE")
            {
                entity.deathDateTime = DateTime.UtcNow;
            }

            if (checkAnimal.visitedLocations.Any() && entity.chippingLocationId == checkAnimal.visitedLocations[0].id)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.Invalid,
                    Description = "Новая точка чипирования совпадает с первой посещенной точкой локации"
                };
            }

            //берет объект из бд и соединяет с изменениями
            var animalUnity = _mapper.Map(entity, checkAnimal);

            //обновляет объект
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
    public async Task<IBaseResponse<bool>> DeleteAnimal(long id)
    {
        try
        {
            var checkAnimal = await _animalRepository.GetById(id);

            if (checkAnimal == null)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.AnimalNotFound,
                    Description = "Животное не найдено"
                };
            }

            var visitedLoc = checkAnimal.visitedLocations;
            
            if (visitedLoc.Any() && 
                visitedLoc.Last().id != checkAnimal.chippingLocationId)
            {
                return new BaseResponse<bool>
                {
                    StatusCode = StatusCode.AnimalLeft,
                    Description = "Животное покинуло локацию чипирования"
                };
            }

            await _animalRepository.Delete(id);
            
            return new BaseResponse<bool>
            {
                StatusCode = StatusCode.OK,
                Data = true
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                Description = $"DeleteAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<DTOAnimal>> AddType(long animalId, long typeId)
    {
        try
        {
            var checkType = await _typeRepository.GetTypeById(typeId);
            if (checkType == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.TypeNotFound,
                    Description = "Тип не найден"
                };
            }

            var checkAnimal = await _animalRepository.GetById(animalId);
            if (checkAnimal == null)
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.AnimalNotFound,
                    Description = "Животное не найдено"
                };
            }

            if (checkAnimal.animalTypes.Contains(checkType))
            {
                return new BaseResponse<DTOAnimal>()
                {
                    StatusCode = StatusCode.TypeAlreadyExist,
                    Description = "Тип уже существует"
                };
            }

            var response = await _animalRepository.AddType(animalId, typeId);

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
                Description = $"AddTypeAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

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