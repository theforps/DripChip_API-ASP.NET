using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class animalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public animalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{animalId:int}")]
        public async Task<ActionResult> GetAnimalById(int animalId)
        {
            if (animalId == null || animalId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
            var animal = await _animalService.GetAnimal(animalId);

            if (animal.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(animal.Description);
            }
            
            return Ok(animal.Data);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult> GetAnimal([FromQuery] DTOAnimalSearch animal)
        {

            if (animal.from < 0 || animal.size <= 0 ||
                animal.chipperId <= 0 || animal.chippingLocationId <= 0 ||
                (animal.lifeStatus != "ALIVE" && animal.lifeStatus != "DEAD") ||
                (animal.gender != "MALE" && animal.gender != "FEMALE" && animal.gender != "OTHER"))
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var response = await _animalService.GetAnimalByParam(animal);

            if (response.Data == null)
            {
                return Ok(response.Description);
            }
            
            return Ok(response.Data);
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("types/{typeId:long}")]
        public async Task<ActionResult> GetAnimalType(long typeId)
        {
            if (typeId <= 0 || typeId == null)
            {
                return BadRequest();
            }

            var response = await _animalService.GetType(typeId);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }
        
    }
}
