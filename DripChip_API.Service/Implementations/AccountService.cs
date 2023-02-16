using AutoMapper;
using DripChip_API.Domain.Enums;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _userRepository;
    private readonly IMapper _mapper;
    public AccountService(IAccountRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IBaseResponse<DTOUser>> GetUser(int id)
    {
        try
        {
            var response = await _userRepository.GetById(id);
            var user = _mapper.Map<DTOUser>(response);
            
            if (user == null)
            {
                return new BaseResponse<DTOUser>()
                {
                    Description = "Пользователь не найден",
                    StatusCode = StatusCode.AccountNotFound
                };
            }

            return new BaseResponse<DTOUser>()
            {
                StatusCode = StatusCode.OK,
                Data = user,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOUser>()
            {
                Description = $"GetUser : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }

    public IBaseResponse<List<DTOUser>> GetUsersByParam(DTOUserSearch userSearch)
    {
        try
        {
            var search = _mapper.Map<User>(userSearch);
            
            var response = _userRepository.GetByParams(search, userSearch.from, userSearch.size);
            
            var user = _mapper.Map<List<DTOUser>>(response);
            
            if (!user.Any())
            {
                return new BaseResponse<List<DTOUser>>()
                {
                    Description = "Пользователи не найдены",
                    StatusCode = StatusCode.AccountNotFound
                };
            }

            return new BaseResponse<List<DTOUser>>()
            {
                StatusCode = StatusCode.OK,
                Data = user,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<DTOUser>>()
            {
                Description = $"GetUsersByParam : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
}