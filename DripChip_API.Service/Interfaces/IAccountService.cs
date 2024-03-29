﻿using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;

namespace DripChip_API.Service.Interfaces;

public interface IAccountService
{
    Task<IBaseResponse<DTOUser>> GetUser(int id);
    IBaseResponse<List<DTOUser>> GetUsersByParam(DTOUserSearch userSearch);
    Task<IBaseResponse<DTOUser>> UpdateUser(int id, DTOUserRegistration user);
    Task<IBaseResponse<bool>> DeleteUser (int id);
}