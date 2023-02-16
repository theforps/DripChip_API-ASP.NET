using DripChip_API.Domain.Enums;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;
using DripChip_API.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.Service.Implementations;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _userRepository;
    public AccountService(IAccountRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IBaseResponse<DTOUser>> GetUser(int id)
    {
        try
        {
            var user = await _userRepository.GetById(id);

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
            var user = _userRepository.GetByParams(userSearch);

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