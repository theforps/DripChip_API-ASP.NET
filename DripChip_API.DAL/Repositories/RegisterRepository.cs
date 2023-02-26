﻿using DripChip_API.DAL.Interfaces;
using DripChip_API.Domain.DTO;
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
    
    public async Task Create(DTOUserRegistration user)
    {
        var account = new User()
        {
            firstName = user.firstName,
            lastName = user.lastName,
            email = user.email,
            password = user.password
        };
        
        await _db.Users.AddAsync(account);
        await _db.SaveChangesAsync();
    }
    public async Task<DTOUser> GetByEmail(string email)
    {
        var result = await _db.Users
            .Select(x => new DTOUser()
            {
                lastName = x.lastName,
                firstName = x.firstName,
                email = x.email,
                id = x.id
            }).AsNoTracking()
            .FirstOrDefaultAsync(x => x.email == email);

        if (result != null)
            return result;

        return new DTOUser();
    }

    public async Task<bool> GetUser(string login, string password)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.email == login && x.password == password);
        
        if (user != null)
        {
            return true;
        }

        return false;
    }
}