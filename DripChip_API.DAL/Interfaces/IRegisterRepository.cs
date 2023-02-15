using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

public interface IRegisterRepository
{
    Task Create(User user);
    IQueryable<User> GetAll();
}