using DripChip_API.Domain.DTO.Animal;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public class AnimalTypesController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public AnimalTypesController(IAnimalService animalService)
        {
            _animalService = animalService;
        }
        
        [HttpPost("{animalId:long?}/types/{typeId:long?}")]
        public async Task<ActionResult> AddTypeToAnimal(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0)
            {
                return BadRequest("Неправильные входные данные");
            }
            else if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var response = await _animalService.AddType(animalId, typeId);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeNotFound ||
                response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.TypeAlreadyExist)
            {
                return Conflict(response.Description);
            }

            return Created("", response.Data);
        }

        [HttpPut("{animalId:long?}/types")]
        public async Task<ActionResult> EditType(long animalId, [FromBody] DTOEditType entity)
        {
            if (animalId <= 0 || !ModelState.IsValid)
            {
                return BadRequest("Неправильные входные данные");
            }
            else if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var response = await _animalService.EditType(animalId, entity);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeNotFound ||
                response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.TypeAlreadyExist)
            {
                return Conflict(response.Description);
            }

            return Ok(response.Data);
        }
        [HttpDelete("{animalId:long?}/types/{typeId:long?}")]
        public async Task<ActionResult> DeleteType(long animalId, long typeId)
        {
            if (animalId <= 0 || typeId <= 0)
            {
                return BadRequest("Неправильные входные данные");
            }
            else if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var response = await _animalService.DeleteType(animalId, typeId);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeNotFound ||
                response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.TypeIsSingle)
            {
                return BadRequest(response.Description);
            }

            return Ok();
        }
    }
}
