using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAccountRepository
{
    Task<User> GetById(int id);
    List<User> GetByParams(User entity, int from, int size);
}