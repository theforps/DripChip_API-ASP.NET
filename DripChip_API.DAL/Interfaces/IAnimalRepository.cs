using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> GetById(long id);
    List<Animal> GetByParams(Animal entity, int from, int size, DateTime start, DateTime end);
    Task<Animal> Add(Animal entity);
    Task<Animal> Update(Animal entity);
    Task Delete(long id);
    Task<Animal> AddType(long animalId, long typeId);
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
}