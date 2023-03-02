using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _db;

    public AccountRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<User> GetUserById(int id)
    {
        var result = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);

        return result;
    }
    
    public List<User> GetUsersByParams(User userSearch, int from, int size)
    {
        var result = _db.Users.AsNoTracking().Where(x =>
                x.firstName.ToLower().Contains(userSearch.firstName.ToLower()) 
                && x.lastName.ToLower().Contains(userSearch.lastName.ToLower()) 
                && x.email.ToLower().Contains(userSearch.email.ToLower()))
            .Skip(from).Take(size).OrderBy(x => x.id).ToList();

        return result;
    }

    public async Task<User> UpdateUser(User entity)
    {
        _db.Users.Update(entity);
        await _db.SaveChangesAsync();
        
        return entity;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var animals = _db.Animals.AsNoTracking().Where(x => x.chipperId == id);

        if (!await animals.AnyAsync())
        {
            var result = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.id == id);

            if (result != null)
            {
                _db.Users.Remove(result);
                await _db.SaveChangesAsync();

                return true;
            }
        }

        return false;
    }
}