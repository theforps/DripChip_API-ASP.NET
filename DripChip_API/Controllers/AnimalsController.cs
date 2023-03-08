using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DripChip_API.Domain.Enums;

namespace DripChip_API.Controllers
{
    #region StatusCodes
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    #endregion
    [Route("animals")]
    [Authorize]
    [ApiController]
    
    public class AnimalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        [HttpGet("{animalId:long?}")]
        public async Task<ActionResult> GetById(long animalId)
        {
            if (animalId <= 0)
            {
                return BadRequest();
            }
            
            var animal = await _animalService.GetAnimal(animalId);

            if (animal.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(animal.Description);
            }

            return Ok(animal.Data);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult> Get([FromQuery] DTOAnimalSearch animal)
        {
            if (!ModelState.IsValid || 
                (animal.lifeStatus != null && 
                 !Enum.IsDefined(typeof(LifeStatus), animal.lifeStatus)) ||
                (animal.gender != null && 
                 !Enum.IsDefined(typeof(Gender), animal.gender)))
            {
                return BadRequest("Неправильные входные данные");
            }

            if (animal.startDateTime != String.Format(animal.startDateTime, "s") || 
                animal.endDateTime != String.Format(animal.endDateTime, "s"))
            {
                return BadRequest("Неправильный формат даты");
            }

            var response = await _animalService.GetAnimalByParam(animal);

            if (response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return BadRequest(response.Description);
            }

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] DTOAnimalAdd entity)
        {
            if (!ModelState.IsValid || !Enum.IsDefined(typeof(Gender), entity.gender))
            {
                return BadRequest("Данные не валидны");
            }
            
            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var response = await _animalService.AddAnimal(entity);

            if (response.StatusCode == Domain.Enums.StatusCode.Invalid)
            {
                return BadRequest(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.NotFound)
            {
                return NotFound(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.Conflict)
            {
                return Conflict(response.Description);
            }

            return Created("", response.Data);
        }

        [HttpPut("{animalId:long?}")]
        public async Task<ActionResult> Update([FromBody] DTOAnimalUpdate entity, long animalId)
        {
            if (animalId <= 0 ||
                !ModelState.IsValid ||
                !Enum.IsDefined(typeof(Gender), entity.gender) ||
                !Enum.IsDefined(typeof(LifeStatus), entity.lifeStatus))
            {
                return BadRequest("Данные не валидны");
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }
            
            var response = await _animalService.UpdateAnimal(entity, animalId);

            if (response.StatusCode == Domain.Enums.StatusCode.Invalid)
            {
                return BadRequest(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.NotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }

        [HttpDelete("{animalId:long?}")]
        public async Task<ActionResult> Delete(long animalId)
        {
            if (animalId <= 0)
            {
                return BadRequest("Неправильные входные данные");
            }

            var response = await _animalService.DeleteAnimal(animalId);

            if (response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.AnimalLeft)
            {
                return BadRequest(response.Description);
            }
            else if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }
            
            return Ok();
        }

        [HttpPost("{animalId:long?}/types/{typeId:long?}")]
        public async Task<ActionResult> AddTypeToAnimal(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0)
            {
                return BadRequest("Неправильные входные данные");
            }
            
            
            
            return Ok();
        }

        #region Location
        [HttpGet("{animalId:long?}/locations")]
        public async Task<ActionResult> GetAnimalLocations(long animalId, [FromQuery] DTOAnimalSearchLocation animalSearchLocation)
        {
            if (!ModelState.IsValid || animalId <= 0)
            {
                return BadRequest();
            }

            if (animalSearchLocation.startDateTime != String.Format(animalSearchLocation.startDateTime, "s") || 
                animalSearchLocation.endDateTime != String.Format(animalSearchLocation.endDateTime, "s"))
            {
                return BadRequest("Неправильный формат даты");
            }
            
            var animal = await _animalService.GetAnimal(animalId);

            if (animal.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(animal.Description);
            }

            var result = await _animalService.GetLocationStory(animalId, animalSearchLocation);
            
            if (result.StatusCode == Domain.Enums.StatusCode.LocationStoryNotFound || result.Data == null)
            {
                return BadRequest(result.Description);
            }
            
            return Ok(result.Data);
        }
        #endregion
    }
}
