using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL.Repositories;

public class LocationInfoRepository: ILocationInfoRepository
{
    private readonly ApplicationDbContext _db;
    public LocationInfoRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public List<LocationInfo> GetAnimalLocations(long id, int from, int size, DateTime start, DateTime end)
    {
        var result = _db.Animals
            .AsNoTracking()
            .Where(x => x.id == id)
            .SelectMany(x => x.visitedLocations.Where(y =>
                    y.dateTimeOfVisitLocationPoint >= start &&
                    y.dateTimeOfVisitLocationPoint <= end)
                .OrderBy(y => y.dateTimeOfVisitLocationPoint)
                .Skip(from)
                .Take(size))
            .Include(x => x.locationPoint)
            .ToList();
        
        return result;
    }

    public async Task<LocationInfo> Get(long visitedLocationId)
    {
        var result = await _db.LocationInfo
            .Include(x => x.locationPoint)
            .FirstOrDefaultAsync(x => x.id == visitedLocationId);

        return result;
    }
    public async Task<List<LocationInfo>> GetList(long animalId)
    {
        var result = _db.LocationInfo
            .Include(x => x.locationPoint)
            .Where(x => x.animal.id == animalId).ToList();

        return result;
    }

    public async Task<LocationInfo> AddToAnimal(long animalId, LocationInfo entity)
    {
        var animal = await _db.Animals.FirstOrDefaultAsync(x => x.id == animalId);

        if (animal.visitedLocations == null)
        {
            animal.visitedLocations = new List<LocationInfo>();
        }
        animal.visitedLocations.Add(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task<LocationInfo> Update(LocationInfo entity)
    {
        var result = _db.LocationInfo.Update(entity).Entity;
        await _db.SaveChangesAsync();

        return result;
    }

    public async Task<bool> Delete(long animalId, long visitedPointId)
    {
        var animal = await _db.Animals
            .Include(x => x.visitedLocations)
            .Include(x => x.animalTypes)
            .FirstOrDefaultAsync(x => x.id == animalId);
        
        var visitedLoc = animal.visitedLocations.FirstOrDefault(x => x.id == visitedPointId);

        if (visitedLoc != null)
        {
            animal.visitedLocations.Remove(visitedLoc);
            await _db.SaveChangesAsync();

            return true;
        }

        return false;
    }
}