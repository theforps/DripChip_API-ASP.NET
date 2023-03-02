using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface ITypeRepository
{
    Task<Types> GetTypeById(long id);
    Task<List<Types>> GetTypesById(List<long> types);
    Task<bool> CheckTypeExist(Types type);
    Task<Types> AddType(Types type);
    Task<Types> UpdateType(Types type);
    Task<bool> DeleteType(long id);
}