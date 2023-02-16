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
    
    public async Task<Animal> GetById(int id)
    {
        var result = await _db.Animals.FirstOrDefaultAsync(x => x.id == id);
        
        return result;
    }
    public List<Animal> GetByParams(Animal animal, int from, int size, DateTime start, DateTime end)
    {
        var result = _db.Animals
            .Where(x => 
                (animal.chipperId == 0 || x.chipperId == animal.chipperId) && 
                (animal.chippingLocationId == 0 || x.chippingLocationId == animal.chippingLocationId) &&
                x.lifeStatus == animal.lifeStatus && x.gender == animal.gender &&
                x.chippingDateTime >= start && x.chippingDateTime <= end)
            .OrderBy(x => x.id).Skip(from-1).Take(size).ToList();

        return result;
    }


}