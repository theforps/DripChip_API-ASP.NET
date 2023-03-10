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
}