using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

using Microsoft.EntityFrameworkCore;

public class RegisterRepository: IRegisterRepository
{
    private readonly ApplicationDbContext _db;

    public RegisterRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task Create(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }
    public async Task<User> GetByEmail(string email)
    {
        var result = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.email == email);

        return result;
    }

    public async Task<bool> GetUser(string login, string password)
    {
        var user = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x 
            => x.email == login && x.password == password);

        return user != null;
    }
}