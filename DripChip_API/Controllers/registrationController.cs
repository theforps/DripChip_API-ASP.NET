using DripChip_API.Domain.DTO;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class registrationController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public registrationController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> Register([FromBody] DTOUserRegistration user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var check = await _registerService.СheckForExistence(user.email.ToLower());

            if (check.StatusCode == Domain.Enums.StatusCode.AccountExists)
            {
                return Conflict(check.Description);
            }

            var response = await _registerService.CreateUser(user);

            return Created($"/accounts/{response.Data.id}", response.Data);
        }
        
    }
}
