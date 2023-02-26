using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface ILocationRepository
{
    Task<Location> GetById(long id);
    Task<Location> AddLocation(Location location);
    Task<bool> CheckExist(Location location);
    Task<Location> UpdateLocation(Location location);
    Task<bool> DeleteLocation(long id);
}