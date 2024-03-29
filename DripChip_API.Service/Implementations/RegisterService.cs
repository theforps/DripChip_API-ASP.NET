﻿using AutoMapper;
using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;

namespace DripChip_API.Service.Implementations;

public class RegisterService : IRegisterService
{
    private readonly IRegisterRepository _registerRepository;
    private readonly IMapper _mapper;

    public RegisterService(IRegisterRepository registerRepository, IMapper mapper)
    {
        _mapper = mapper;
        _registerRepository = registerRepository;
    }
    
    public async Task<IBaseResponse<DTOUser>> CreateUser(DTOUserRegistration entity)
    {
        try
        {
            var user = _mapper.Map<User>(entity);
            
            await _registerRepository.Create(user);

            var response = await _registerRepository.GetByEmail(user.email);

            var result = _mapper.Map<DTOUser>(response);

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

    public async Task<IBaseResponse<bool>> GetUser(string login, string password)
    {
        try
        {
            var result = await _registerRepository.GetUser(login, password);

            if (result)
            {
                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Data = true
                };   
            }
            
            return new BaseResponse<bool>()
            {
                Description = "Account is not exists",
                StatusCode = StatusCode.AccountIsNotExists,
                Data = false
            };   
        }
        catch (Exception ex)
        {
            return new BaseResponse<bool>()
            {
                Description = $"[GetUser] : {ex.Message}",
                StatusCode = StatusCode.ServerError
            };
        }
    }
}