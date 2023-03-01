using DripChip_API.Domain.DTO;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("register")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegistrationController(IRegisterService registerService)
        {
            _registerService = registerService;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] DTOUserRegistration user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Невалидные входные данные");
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            
            var check = await _registerService.СheckForExistence(user.email);

            if (check.StatusCode == Domain.Enums.StatusCode.AccountExists)
            {
                return Conflict(check.Description);
            }

            var response = await _registerService.CreateUser(user);

            return Created($"/accounts/{response.Data.id}", response.Data);
        }
        
    }
}
