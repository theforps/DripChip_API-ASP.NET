﻿using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.DTO.Type;
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
    
    public async Task<IBaseResponse<Animal>> GetAnimal(long id)
    {
        try
        {
            var animal = await _animalRepository.GetById(id);

            if (animal == null)
            {
                return new BaseResponse<Animal>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            
            return new BaseResponse<Animal>()
            {
                StatusCode = StatusCode.OK,
                Data = animal
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Animal>()
            {
                Description = $"GetAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
        
            
    }
    
    public async Task<IBaseResponse<List<Animal>>> GetAnimalByParam(DTOAnimalSearch animal)
    {
        try
        {
            var search = _mapper.Map<Animal>(animal);
            
            var animals = _animalRepository.GetByParams(
                search, 
                animal.from, 
                animal.size, 
                DateTime.Parse(animal.startDateTime), 
                DateTime.Parse(animal.endDateTime));

            if (!animals.Any())
            {
                return new BaseResponse<List<Animal>>()
                {
                    Description = "Животные не найдены",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }

            return new BaseResponse<List<Animal>>()
            {
                StatusCode = StatusCode.OK,
                Data = animals,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<Animal>>()
            {
                Description = $"GetAnimalsByParam : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
    
    #endregion

    #region Type

    public async Task<IBaseResponse<DTOType>> GetType(long id)
    {
        try
        {
            var response = await _animalRepository.GetTypeById(id);

            var type = _mapper.Map<DTOType>(response);

            if (type.type == null)
            {
                return new BaseResponse<DTOType>()
                {
                    Description = "Тип животного не найден",
                    StatusCode = StatusCode.TypeNotFound
                };
            }

            return new BaseResponse<DTOType>()
            {
                StatusCode = StatusCode.OK,
                Data = type,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOType>()
            {
                Description = $"GetType : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<DTOType>> AddType(DTOTypeInsert entity)
    {
        try
        {
            var type = _mapper.Map<Types>(entity);

            var check = await _animalRepository.CheckTypeExist(type);

            if (check)
            {
                return new BaseResponse<DTOType>()
                {
                    Description = "Тип животного уже существует",
                    StatusCode = StatusCode.TypeAlreadyExist
                };
            }
            
            var response = await _animalRepository.AddType(type);

            var result = _mapper.Map<DTOType>(response);
            
            return new BaseResponse<DTOType>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOType>()
            {
                Description = $"AddType : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<DTOType>> UpdateType(long id, DTOTypeInsert entity)
    {
        try
        {
            var type = _mapper.Map<Types>(entity);

            var check = await _animalRepository.CheckTypeExist(type);

            if (check)
            {
                return new BaseResponse<DTOType>()
                {
                    Description = "Тип животного уже существует",
                    StatusCode = StatusCode.TypeAlreadyExist
                };
            }
            
            type.id = id;
            
            var response = await _animalRepository.UpdateType(type);

            var result = _mapper.Map<DTOType>(response);
            
            return new BaseResponse<DTOType>()
            {
                StatusCode = StatusCode.OK,
                Data = result,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOType>()
            {
                Description = $"UpdateType : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public async Task<IBaseResponse<bool>> DeleteType(long id)
    {
        try
        {

            var result = await _animalRepository.DeleteType(id);

            if (!result)
            {
                return new BaseResponse<bool>()
                {
                    Description = "Тип связан с животным",
                    StatusCode = StatusCode.TypeRelated
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
                Description = $"DeleteType : {ex.Message}",
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