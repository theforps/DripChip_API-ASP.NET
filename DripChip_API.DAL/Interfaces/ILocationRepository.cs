using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface ILocationRepository
{
    Task<Location> GetById(long id);
}