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
        var result = await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.id == id);
        
        return result;
    }

    public async Task<Location> AddLocation(Location location)
    {
        await _db.Locations.AddAsync(location);
        await _db.SaveChangesAsync();

        var result = await _db.Locations.FirstOrDefaultAsync(x => 
            x.latitude.Equals(location.latitude) && x.longitude.Equals(location.longitude));


        return result;

    }

    public async Task<bool> CheckExist(Location location)
    {
        var result = await _db.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
            x.latitude.Equals(location.latitude) && 
            x.longitude.Equals(location.longitude));

        return result != null;
    }

    public async Task<Location> UpdateLocation(Location location)
    {
        _db.Locations.Update(location);
        await _db.SaveChangesAsync();

        return location;
    }

    public async Task<bool> DeleteLocation(long id)
    {
        var checkChipLoc = await _db.Animals.Where(x => x.chippingLocationId == id).AnyAsync();
        var checkVisitedLoc = await _db.LocationInfo.Where(x => x.locationPoint.id == id).AnyAsync();

        if (!checkChipLoc && !checkVisitedLoc)
        {
            var entity = await GetById(id);
            
            _db.Locations.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        return false;
    }
}