using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;

namespace DripChip_API.Service.Interfaces;

public interface IAnimalService
{
    Task<IBaseResponse<DTOAnimal>> GetAnimal(long id);
    Task<IBaseResponse<List<DTOAnimal>>> GetAnimalByParam(DTOAnimalSearch entity);
    Task<IBaseResponse<DTOAnimal>> AddAnimal(DTOAnimalAdd entity);
    Task<IBaseResponse<DTOAnimal>> UpdateAnimal(DTOAnimalUpdate entity);
    Task<IBaseResponse<List<DTOLocationInfo>>> GetLocationStory(long id, DTOAnimalSearchLocation entity);
}