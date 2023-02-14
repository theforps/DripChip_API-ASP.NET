using DripChip_API.DAL.Response;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Interfaces;

public interface IAnimalService
{
    Task<IBaseResponse<Animal>> GetAnimal(int id);
}