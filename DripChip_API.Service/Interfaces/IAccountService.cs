using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Interfaces;

public interface IAccountService
{
    Task<IBaseResponse<DTOUser>> GetUser(int id);
    IBaseResponse<List<DTOUser>> GetUsersByParam(DTOUserSearch userSearch);
}