using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

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

    public IQueryable<User> GetAll()
    {
        return _db.Users;
    }
}