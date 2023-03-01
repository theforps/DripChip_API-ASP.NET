
using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IRegisterRepository
{
    Task Create(User user);
    Task<User> GetByEmail(string email);
    Task<bool> GetUser(string login, string password);
}