using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

public class AnimalRepository : IAnimalRepository
{
    private readonly ApplicationDbContext _db;

    public AnimalRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Animal> GetById(long id)
    {
        var result = await _db.Animals
            .Include(x => x.visitedLocations)
            .Include(x => x.animalTypes)
            .FirstOrDefaultAsync(x => x.id == id);
        
        if (result != null)
            return result;

        return new Animal();
    }
    public List<Animal> GetByParams(Animal animal, int from, int size, DateTime start, DateTime end)
    {
        var result = _db.Animals
            .Include(x => x.visitedLocations)
            .Include(x => x.animalTypes)
            .Where(x => 
                (animal.chipperId == 0 || x.chipperId == animal.chipperId) && 
                (animal.chippingLocationId == 0 || x.chippingLocationId == animal.chippingLocationId) &&
                (x.lifeStatus == animal.lifeStatus || String.IsNullOrEmpty(animal.lifeStatus)) && 
                (x.gender == animal.gender || String.IsNullOrEmpty(animal.gender)) &&
                (x.chippingDateTime >= start && x.chippingDateTime <= end))
            .OrderBy(x => x.id)
            .Skip(from)
            .Take(size)
            .ToList();

        return result;
    }

    public async Task<Types> GetTypeById(long id)
    {
        var result = await _db.Types.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);

        if (result != null)
            return result;

        return new Types();
    }

    public async Task<bool> CheckTypeExist(Types type)
    {
        var check = await _db.Types
            .AsNoTracking()
            .FirstOrDefaultAsync(x => 
                x.type.ToLower().Equals(type.type.ToLower()));

        return check != null;
    }

    public async Task<Types> AddType(Types type)
    {
        var result = _db.Types.Add(type).Entity;
        await _db.SaveChangesAsync();
        
        return result;
    }

    public async Task<Types> UpdateType(Types type)
    {
        _db.Types.Update(type);
        await _db.SaveChangesAsync();
        
        return type;
    }

    public async Task<bool> DeleteType(long id)
    {
        var checkAnimal = await _db.Animals.AnyAsync(x => x.animalTypes.Any(y => y.id == id));

        if (!checkAnimal)
        {
            var entity = await GetTypeById(id);
            
            _db.Types.Remove(entity);
            await _db.SaveChangesAsync();

            return true;
        }

        return false;
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