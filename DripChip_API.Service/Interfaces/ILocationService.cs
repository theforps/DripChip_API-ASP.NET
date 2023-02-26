using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Interfaces;

public interface ILocationService
{
    Task<IBaseResponse<Location>> GetById(long id);
    Task<IBaseResponse<Location>> AddLoc(DTOLocation location);
    Task<IBaseResponse<Location>> UpdateLoc(long id, DTOLocation location);
    Task<IBaseResponse<bool>> DeleteLoc(long id);
}