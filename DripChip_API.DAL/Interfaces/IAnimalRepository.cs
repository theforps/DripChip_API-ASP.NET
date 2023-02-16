using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAnimalRepository
{
    Task<Animal> GetById(int id);
    List<Animal> GetByParams(Animal entity, int from, int size, DateTime start, DateTime end);
    Task<Types> GetTypeById(long id);
}