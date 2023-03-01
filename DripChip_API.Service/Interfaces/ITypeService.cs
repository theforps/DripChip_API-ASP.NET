using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Type;

namespace DripChip_API.Service.Interfaces;

public interface ITypeService
{
    Task<IBaseResponse<DTOType>> GetType(long typeId);
    Task<IBaseResponse<DTOType>> AddType(DTOTypeInsert type);
    Task<IBaseResponse<DTOType>> UpdateType(long id, DTOTypeInsert type);
    Task<IBaseResponse<bool>> DeleteType(long id);
}