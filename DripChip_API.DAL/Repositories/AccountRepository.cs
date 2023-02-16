using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.DTO;

namespace DripChip_API.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _db;

    public AccountRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<DTOUser> GetById(int id)
    {
        var result = await _db.Users
            .Where(x => x.id == id)
            .Select(x => new DTOUser()
            {
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName,
                email = x.email
            })
            .FirstOrDefaultAsync();
        
        return  result;
    }
    
    public List<DTOUser> GetByParams(DTOUserSearch userSearch)
    {
        var result = _db.Users
            .Where(x =>
                x.firstName.Contains(userSearch.firstName) 
                && x.lastName.Contains(userSearch.lastName) 
                && x.email.Contains(userSearch.email))
            .Skip(userSearch.from)
            .Take(userSearch.size)
            .OrderBy(x => x.id)
            .Select(x => new DTOUser()
            {
                email = x.email,
                id = x.id,
                firstName = x.firstName,
                lastName = x.lastName
            }).ToList();

        return result;
    }
}