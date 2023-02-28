using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Domain.DTO.Type;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ApiController]
    public class animalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public animalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        #region Animal
        
        [HttpGet("{animalId:long?}")]
        public async Task<ActionResult> GetAnimalById(long animalId)
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
        public async Task<ActionResult> GetAnimal([FromQuery] DTOAnimalSearch animal)
        {

            if (!ModelState.IsValid || (animal.lifeStatus != null && animal.lifeStatus != "ALIVE" && animal.lifeStatus != "DEAD") ||
                (animal.gender != null && animal.gender != "MALE" && animal.gender != "FEMALE" && animal.gender != "OTHER"))
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
        #endregion

        #region Type
        
        [HttpGet("types/{typeId:long?}")]
        public async Task<ActionResult> GetType(long typeId)
        {
            if (typeId <= 0)
            {
                return BadRequest();
            }

            var response = await _animalService.GetType(typeId);

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Неверные авторизационные данные");
            }
            
            if (response.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }

        [HttpPost("types")]
        public async Task<ActionResult> AddType([FromBody] DTOTypeInsert type)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var result = await _animalService.AddType(type);

            if (result.StatusCode == Domain.Enums.StatusCode.TypeAlreadyExist)
            {
                return Conflict(result.Description);
            }
            
            return Created("types/{typeId:long?}", result.Data);
        }

        [HttpPut("types/{typeId:long?}")]
        public async Task<ActionResult> UpdateType(long typeId, [FromBody] DTOTypeInsert type)
        {
            if (!ModelState.IsValid || typeId <= 0)
            {
                return BadRequest();
            }
            
            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var check = await _animalService.GetType(typeId);

            if (check.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(check.Description);
            }

            var response = await _animalService.UpdateType(typeId, type);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeAlreadyExist)
            {
                return Conflict(response.Description);
            }
            
            return Ok(response.Data);
        }

        [HttpDelete("types/{typeId:long?}")]
        public async Task<ActionResult> DeleteType(long typeId)
        {
            if (typeId <= 0)
            {
                return BadRequest();
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var check = await _animalService.GetType(typeId);

            if (check.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(check.Description);
            }

            var response = await _animalService.DeleteType(typeId);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeRelated)
            {
                return BadRequest(response.Description);
            }
            
            return Ok(response.Data);
        }
        
        #endregion
        
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
