using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;

namespace DripChip_API.Service.Interfaces;

public interface IRegisterService
{
    Task<IBaseResponse<DTOUser>> CreateUser(DTOUserRegistration user);
    Task<IBaseResponse<bool>> СheckForExistence(string email);
}