using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _db;

    public LocationRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Location> GetById(long id)
    {
        var result = await _db.Locations.FirstOrDefaultAsync(x => x.id == id);

        return result;
    }
}