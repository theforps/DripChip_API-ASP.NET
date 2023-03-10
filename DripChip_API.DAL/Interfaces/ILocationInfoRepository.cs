using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

using Domain.DTO.Location;

public interface ILocationInfoRepository
{
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
    Task<LocationInfo> Add(long animalId, LocationInfo entity);
}