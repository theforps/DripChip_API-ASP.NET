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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Register([FromBody] DTOUserRegistration user)
        {
            if (String.IsNullOrEmpty(user.firstName)
                || String.IsNullOrWhiteSpace(user.firstName)
                || String.IsNullOrEmpty(user.lastName)
                || String.IsNullOrWhiteSpace(user.lastName)
                || String.IsNullOrEmpty(user.email)
                || String.IsNullOrWhiteSpace(user.email)
                || String.IsNullOrEmpty(user.password)
                || String.IsNullOrWhiteSpace(user.password)
                || !ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var check = await _registerService.СheckForExistence(user.email);

            if (check.StatusCode == Domain.Enums.StatusCode.AccountExists)
            {
                return Conflict(check.Description);
            }

            var response = await _registerService.CreateUser(user);

            return Ok(response.Data);
        }
        
    }
}
