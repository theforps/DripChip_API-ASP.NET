using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Location;

namespace DripChip_API.Service.Interfaces;

public interface ILocationInfoService
{
    Task<IBaseResponse<List<DTOLocationInfo>>> GetLocationStory(long id, DTOAnimalSearchLocation entity);
}