using DripChip_API.Domain.DTO;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class accountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public accountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{accountId:int?}")]
        public async Task<ActionResult> GetUserById(int accountId)
        {
            if (accountId <= 0)
            {
                return BadRequest();
            }

            var response = await _accountService.GetUser(accountId);

            if (response.StatusCode == Domain.Enums.StatusCode.AccountNotFound)
            {
                return NotFound(response.Description);
            }

            return Ok(response.Data);
            
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("search")]
        public ActionResult GetUserByParams([FromQuery] DTOUserSearch userSearch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = _accountService.GetUsersByParam(userSearch);

            if (response.StatusCode == Domain.Enums.StatusCode.AccountNotFound)
            {
                return BadRequest(response.Description);
            }
            
            return Ok(response.Data);
        }
    }
}
