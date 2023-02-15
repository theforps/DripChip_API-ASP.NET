using DripChip_API.DAL.Interfaces;
using DripChip_API.DAL.Response;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.Enums;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.Service.Implementations;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;

    public AnimalService(IAnimalRepository animalRepository)
    {
        _animalRepository = animalRepository;
    }
    
    public async Task<IBaseResponse<Animal>> GetAnimal(int id)
    {
        try
        {
            var animal = await _animalRepository.GetAll().FirstOrDefaultAsync(x => x.id == id);

            if (animal == null)
            {
                return new BaseResponse<Animal>()
                {
                    Description = "Животное не найдено",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }
            
            return new BaseResponse<Animal>()
            {
                StatusCode = StatusCode.OK,
                Data = animal
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<Animal>()
            {
                Description = $"GetAnimal : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
        
            
    }
    
    public async Task<IBaseResponse<List<Animal>>> GetAnimalByParam(DTOAnimalSearch animal)
    {
        try
        {
            var animals = _animalRepository.GetAll().Where(x =>
                  (animal.chipperId == null || x.chipperId == animal.chipperId) && 
                  (animal.chippingLocationId == null || x.chippingLocationId == animal.chippingLocationId) &&
                  x.lifeStatus == animal.lifeStatus &&
                  x.gender == animal.gender &&
                  x.chippingDateTime >= animal.startDateTime &&
                  x.chippingDateTime <= animal.endDateTime)
                  .OrderBy(x => x.id)
                  .Skip(animal.from-1)
                  .Take(animal.size)
                  .ToList();

            if (!animals.Any())
            {
                return new BaseResponse<List<Animal>>()
                {
                    Description = "Животные не найдены",
                    StatusCode = StatusCode.AnimalNotFound
                };
            }

            return new BaseResponse<List<Animal>>()
            {
                StatusCode = StatusCode.OK,
                Data = animals,
            };
        }
        catch (Exception ex)
        {
            return new BaseResponse<List<Animal>>()
            {
                Description = $"GetAnimalsByParam : {ex.Message}",
                StatusCode = StatusCode.ServerError,
            };
        }
    }
}