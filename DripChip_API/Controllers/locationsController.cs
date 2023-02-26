using DripChip_API.Domain.DTO.Location;
using DripChip_API.Domain.Models;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace DripChip_API.Controllers
{
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class locationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public locationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        
        [HttpGet("{pointId:long?}")]
        public async Task<ActionResult> GetLocationById(long pointId)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Unauthorized("Неверные авторизационные данные");
            }
            
            if (pointId <= 0)
            {
                return BadRequest("Невалидные данные");
            }

            var response = await _locationService.GetById(pointId);

            if (response.StatusCode == Domain.Enums.StatusCode.LocationNotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<ActionResult> AddLocation([FromBody] DTOLocation location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидные данные");
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var response = await _locationService.AddLoc(location);

            if (response.StatusCode == Domain.Enums.StatusCode.LocationAlreadyExist)
            {
                return Conflict(response.Description);
            }
            
            return Created("{pointId}", response.Data);
        }

        [HttpPut("{pointId:long?}")]
        public async Task<ActionResult> UpdateLoc(long pointId,[FromBody] DTOLocation location)
        {
            if (!ModelState.IsValid || pointId <= 0)
            {
                return BadRequest("Невалидные данные");
            }
            
            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }

            var findLoc = await _locationService.GetById(pointId);

            if (findLoc.StatusCode == Domain.Enums.StatusCode.LocationNotFound)
            {
                return NotFound(findLoc.Description);
            }

            var response = await _locationService.UpdateLoc(pointId, location);

            if (response.StatusCode == Domain.Enums.StatusCode.LocationAlreadyExist)
            {
                return Conflict(response.Description);
            }

            return Ok(response.Data);

        }

        [HttpDelete("{pointId:long?}")]
        public async Task<ActionResult> DeleteLoc(long pointId)
        {
            if (pointId <= 0)
            {
                return BadRequest("Невалидные данные");
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Не авторизован");
            }
            
            var result = await _locationService.DeleteLoc(pointId);

            if (result.StatusCode == Domain.Enums.StatusCode.LocationRelated)
            {
                return BadRequest("Связана с животным");
            }

            if (result.StatusCode == Domain.Enums.StatusCode.LocationNotFound)
            {
                return NotFound(result.Description);
            }

            return Ok();
        }
    }
}
