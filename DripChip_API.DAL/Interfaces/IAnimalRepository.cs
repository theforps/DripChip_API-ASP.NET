using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

using Domain.DTO.Animal;

public interface IAnimalRepository
{
    Task<Animal> GetById(int id);
    List<Animal> GetByParams(DTOAnimalSearch animal);
}