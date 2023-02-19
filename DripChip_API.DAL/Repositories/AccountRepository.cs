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
        var result = await _db.Users.FirstOrDefaultAsync(x => x.id == id);
        
        return  result;
    }
    
    public List<User> GetUsersByParams(User userSearch, int from, int size)
    {
        var result = _db.Users.Where(x =>
                x.firstName.ToLower().Contains(userSearch.firstName.ToLower()) 
                && x.lastName.ToLower().Contains(userSearch.lastName.ToLower()) 
                && x.email.ToLower().Contains(userSearch.email.ToLower()))
            .Skip(from).Take(size).OrderBy(x => x.id).ToList();

        return result;
    }
}