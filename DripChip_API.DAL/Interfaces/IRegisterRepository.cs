using DripChip_API.Domain.Models;

namespace DripChip_API.DAL.Interfaces;

using Domain.DTO;

public interface IRegisterRepository
{
    Task Create(DTOUserRegistration user);
    Task<DTOUser> GetByEmail(string email);
}