using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class locationsController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public locationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{pointId:long?}")]
        public async Task<ActionResult> GetLocationById(long pointId)
        {
            if (pointId <= 0)
            {
                return BadRequest();
            }

            var response = await _locationService.GetById(pointId);

            if (response.StatusCode == Domain.Enums.StatusCode.LocationNotFound)
            {
                return NotFound(response.Description);
            }
            
            return Ok(response.Data);
        }
    }
}
