using System.Diagnostics;
using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.DTO;
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _db;

    public AccountRepository(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IQueryable<User> GetAll()
    {
        return _db.Users;
    }
}