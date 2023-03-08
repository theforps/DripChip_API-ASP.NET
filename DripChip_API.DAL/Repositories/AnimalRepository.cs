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

        return result;
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

    public async Task<Animal> Add(Animal entity)
    {
        var result = _db.Animals.AddAsync(entity).Result.Entity;
        await _db.SaveChangesAsync();

        return result;
    }

    public async Task<Animal> Update(Animal entity)
    {
        _db.Animals.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    public async Task Delete(long id)
    {
        var animal = await _db.Animals.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);
        
        _db.Animals.Remove(animal);
        await _db.SaveChangesAsync();
    }

    public async Task<Animal> AddType(long animalId ,long typeId)
    {
        var animal = await _db.Animals.AsNoTracking().FirstOrDefaultAsync(x => x.id == animalId);
        var type = await _db.Types.AsNoTracking().FirstOrDefaultAsync(x => x.id == typeId);
        
        animal.animalTypes.Add(type);
        await _db.SaveChangesAsync();
        
        var result = await _db.Animals.AsNoTracking().FirstOrDefaultAsync(x => x.id == animalId);

        return result;
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