using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Type;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class TypeService : ITypeService
{
    private readonly ITypeRepository _typeRepository;
    private readonly IMapper _mapper;
    
    public TypeService(ITypeRepository typeRepository, IMapper mapper)
    {
        _mapper = mapper;
        _typeRepository = typeRepository;
    }

    public async Task<IBaseResponse<DTOType>> GetType(long id)
    {
        try
        {
            var response = await _typeRepository.GetTypeById(id);

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

            var check = await _typeRepository.CheckTypeExist(type);

            if (check)
            {
                return new BaseResponse<DTOType>()
                {
                    Description = "Тип животного уже существует",
                    StatusCode = StatusCode.TypeAlreadyExist
                };
            }
            
            var response = await _typeRepository.AddType(type);

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

            var check = await _typeRepository.CheckTypeExist(type);

            if (check)
            {
                return new BaseResponse<DTOType>()
                {
                    Description = "Тип животного уже существует",
                    StatusCode = StatusCode.TypeAlreadyExist
                };
            }
            
            type.id = id;
            
            var response = await _typeRepository.UpdateType(type);

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

            var result = await _typeRepository.DeleteType(id);

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
}