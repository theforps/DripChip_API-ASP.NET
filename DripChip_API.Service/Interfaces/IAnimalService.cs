using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.DTO.Type;
using DripChip_API.Domain.Models;

namespace DripChip_API.Service.Interfaces;

public interface IAnimalService
{
    Task<IBaseResponse<Animal>> GetAnimal(long id);
    Task<IBaseResponse<List<Animal>>> GetAnimalByParam(DTOAnimalSearch animal);
    Task<IBaseResponse<DTOType>> GetType(long typeId);
    Task<IBaseResponse<List<DTOLocationInfo>>> GetLocationStory(long id, DTOAnimalSearchLocation animal);
}