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
            var account = new User()
            {
                firstName = user.firstName,
                lastName = user.lastName,
                email = user.email,
                password = user.password
            };

            await _registerRepository.Create(account);

            var result = await _registerRepository.GetAll()
                .FirstOrDefaultAsync(x => x.email == user.email);

            return new BaseResponse<DTOUser>()
            {
                StatusCode = StatusCode.OK,
                Data = new DTOUser()
                {
                    id = result.id,
                    firstName = result.firstName,
                    lastName = result.lastName,
                    email = result.email
                }
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
            var result = await _registerRepository.GetAll().FirstOrDefaultAsync(x => x.email == email);

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