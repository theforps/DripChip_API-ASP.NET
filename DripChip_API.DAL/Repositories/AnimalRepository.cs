using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

using Domain.DTO.Animal;
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
    public List<Animal> GetByParams(DTOAnimalSearch animal)
    {
        var result = _db.Animals.Where(x =>
                (animal.chipperId == null || x.chipperId == animal.chipperId) && 
                (animal.chippingLocationId == null || x.chippingLocationId == animal.chippingLocationId) &&
                x.lifeStatus == animal.lifeStatus &&
                x.gender == animal.gender &&
                x.chippingDateTime >= animal.startDateTime &&
                x.chippingDateTime <= animal.endDateTime)
            .OrderBy(x => x.id)
            .Skip(animal.from-1)
            .Take(animal.size)
            .ToList();

        return result;
    }


}