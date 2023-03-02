using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL.Repositories;

public class TypeRepository : ITypeRepository
{
    private readonly ApplicationDbContext _db;

    public TypeRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<Types> GetTypeById(long id)
    {
        var result = await _db.Types.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);

        if (result != null)
            return result;

        return new Types();
    }

    public async Task<List<Types>> GetTypesById(List<long> types)
    {
        var findTypes = _db.Types.Where(x => types.Contains(x.id)).ToList();

        return findTypes;
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
}