using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface ILocationInfoRepository
{
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
    Task<LocationInfo> Get(long visitedLocationId);
    Task<List<LocationInfo>> GetList(long animalId);
    Task<LocationInfo> AddToAnimal(long animalId, LocationInfo entity);
    Task<LocationInfo> Update(LocationInfo entity);
    Task<bool> Delete(long animalId, long visitedPointId);
}