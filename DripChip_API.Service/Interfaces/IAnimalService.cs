using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;

namespace DripChip_API.Service.Interfaces;

public interface IAnimalService
{
    Task<IBaseResponse<DTOAnimal>> GetAnimal(long id);
    Task<IBaseResponse<List<DTOAnimal>>> GetAnimalByParam(DTOAnimalSearch entity);
    Task<IBaseResponse<DTOAnimal>> AddAnimal(DTOAnimalAdd entity);
    Task<IBaseResponse<DTOAnimal>> UpdateAnimal(DTOAnimalUpdate entity, long id);
    Task<IBaseResponse<bool>> DeleteAnimal(long id);
    Task<IBaseResponse<DTOAnimal>> AddType(long animalId, long typeId);
    Task<IBaseResponse<DTOAnimal>> EditType(long animalId, DTOEditType entity);
    Task<IBaseResponse<DTOAnimal>> DeleteType(long animalId, long typeId);
}