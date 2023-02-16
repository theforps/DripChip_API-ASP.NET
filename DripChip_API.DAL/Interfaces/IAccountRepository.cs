namespace DripChip_API.DAL.Interfaces;

using Domain.DTO;

public interface IAccountRepository
{
    Task<DTOUser> GetById(int id);
    List<DTOUser> GetByParams(DTOUserSearch userSearch);
}