using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface ILocationInfoRepository
{
    List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end);
}