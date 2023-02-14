using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

public class AnimalRepository : IAnimalRepository
{
    private readonly ApplicationDbContext _db;

    public AnimalRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IQueryable<Animal> GetAll()
    {
        return _db.Animals;
    }
}