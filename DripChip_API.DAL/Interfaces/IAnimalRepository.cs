using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> GetById(long id);
    List<Animal> GetByParams(Animal entity, int from, int size, DateTime start, DateTime end);
    Task<Animal> Add(Animal entity);
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
}