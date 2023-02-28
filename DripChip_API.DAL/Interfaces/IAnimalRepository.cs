using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> GetById(long id);
    List<Animal> GetByParams(Animal entity, int from, int size, DateTime start, DateTime end);
    Task<Types> GetTypeById(long id);
    Task<bool> CheckTypeExist(Types type);
    Task<Types> AddType(Types type);
    Task<Types> UpdateType(Types type);
    Task<bool> DeleteType(long id);
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
}