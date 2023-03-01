using DripChip_API.Domain.DTO.Type;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Authorize]
    [Route("animals")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet("types/{typeId:long?}")]
        public async Task<ActionResult> GetType(long typeId)
        {
            if (typeId <= 0)
            {
                return BadRequest();
            }

            var response = await _typeService.GetType(typeId);

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

            var result = await _typeService.AddType(type);

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

            var check = await _typeService.GetType(typeId);

            if (check.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(check.Description);
            }

            var response = await _typeService.UpdateType(typeId, type);

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

            var check = await _typeService.GetType(typeId);

            if (check.StatusCode == Domain.Enums.StatusCode.TypeNotFound)
            {
                return NotFound(check.Description);
            }

            var response = await _typeService.DeleteType(typeId);

            if (response.StatusCode == Domain.Enums.StatusCode.TypeRelated)
            {
                return BadRequest(response.Description);
            }
            
            return Ok(response.Data);
        }
    }
}
