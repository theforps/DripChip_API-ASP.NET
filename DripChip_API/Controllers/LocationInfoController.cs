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
    [ApiController]
    [Authorize]
    public class LocationInfoController : ControllerBase
    {
        private readonly ILocationInfoService _locationInfoService;
        public LocationInfoController(ILocationInfoService locationInfoService)
        {
            _locationInfoService = locationInfoService;
        }
        
        [HttpGet("{animalId:long?}/locations")]
        public async Task<ActionResult> GetAnimalLocations(long animalId, [FromQuery] DTOAnimalSearchLocation animalSearchLocation)
        {
            if (!ModelState.IsValid || animalId <= 0 ||
                animalSearchLocation.startDateTime != String.Format(animalSearchLocation.startDateTime, "s")|| 
                animalSearchLocation.endDateTime != String.Format(animalSearchLocation.endDateTime, "s")) 
            {
                return BadRequest("Неправильные входные данные");
            }

            var response = await _locationInfoService.GetLocationStory(animalId, animalSearchLocation);
            
            if (response.StatusCode == Domain.Enums.StatusCode.LocationStoryNotFound)
            {
                return BadRequest(response.Description);
            }
            else if (response.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }
    }
}
