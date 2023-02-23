using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAccountRepository
{
    Task<User> GetUserById(int id);
    List<User> GetUsersByParams(User entity, int from, int size);
    Task<User> UpdateUser(User entity);
    Task<bool> DeleteUser (int id);
}