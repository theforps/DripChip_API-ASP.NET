using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.Service.Implementations;

public class RegisterService : IRegisterService
{
    private readonly IRegisterRepository _registerRepository;

    public RegisterService(IRegisterRepository registerRepository)
    {
        _registerRepository = registerRepository;
    }
    
    public async Task<IBaseResponse<DTOUser>> CreateUser(DTOUserRegistration user)
    {
        try
        {
            await _registerRepository.Create(user);

            var result = await _registerRepository.GetByEmail(user.email);

            return new BaseResponse<DTOUser>()
            {
                StatusCode = StatusCode.OK,
                Data = result
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<DTOUser>()
            {
                Description = $"[Create] : {ex.Message}",
                StatusCode = StatusCode.ServerError
            };
        }
    }

    public async Task<IBaseResponse<bool>> СheckForExistence(string email)
    {
        try
        {
            var result = await _registerRepository.GetByEmail(email.ToLower());

            if (result == null)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Data = false
                };   
            }
            
            return new BaseResponse<bool>()
            {
                Description = "Account already exists",
                StatusCode = StatusCode.AccountExists,
                Data = true
            };   
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                Description = $"[Check] : {ex.Message}",
                StatusCode = StatusCode.ServerError
            };
        }
    }
}