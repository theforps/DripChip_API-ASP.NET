using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAccountRepository
{
    IQueryable<User> GetAll();
}