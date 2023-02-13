using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IAccountRepository<T>
{
    IQueryable<T> GetAll();
}