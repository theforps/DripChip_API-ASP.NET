using DripChip_API.Domain.DTO;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    [Route("accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IRegisterService _registerService;

        public AccountsController(IAccountService accountService, IRegisterService registerService)
        {
            _registerService = registerService;
            _accountService = accountService;
        }

        #region CRUD Account
        
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
        
        [HttpGet("search")]
        public ActionResult GetUserByParams([FromQuery] DTOUserSearch userSearch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Неверные входные данные");
            }

            var response = _accountService.GetUsersByParam(userSearch);

            if (response.StatusCode == Domain.Enums.StatusCode.AccountNotFound)
            {
                return BadRequest(response.Description);
            }
            
            return Ok(response.Data);
        }
        
        [HttpPut("{accountId:int?}")]
        public async Task<ActionResult> UpdateUser(int accountId, [FromBody] DTOUserRegistration user)
        {
            if (!ModelState.IsValid || accountId <= 0)
            {
                return BadRequest("Невалидные входные данные");
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Необходимо авторизоваться");
            }

            var findAccount = await _accountService.GetUser(accountId);

            if (findAccount.StatusCode == Domain.Enums.StatusCode.AccountNotFound ||
                findAccount.Data.email != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }

            var checkExist = await _registerService.СheckForExistence(user.email);

            if (checkExist.StatusCode == Domain.Enums.StatusCode.AccountExists &&
                user.email != HttpContext.User.Identity.Name)
            {
                return Conflict(checkExist.Description);
            }

            var result = await _accountService.UpdateUser(accountId, user);

            if (result.StatusCode == Domain.Enums.StatusCode.OK)
            {
                return Ok(result.Data);
            }
            
            return BadRequest("Что-то пошло не так");
        }
        
        [HttpDelete("{accountId:int?}")]
        public async Task<ActionResult> DeleteUser(int accountId)
        {
            if (accountId <= 0)
            {
                return BadRequest("Невалидный id");
            }

            if (HttpContext.User.Identity.Name == Domain.Enums.StatusCode.AuthorizationDataIsEmpty.ToString())
            {
                return Unauthorized("Необходимо авторизоваться");
            }
            
            var result = await _accountService.GetUser(accountId);

            if (result.StatusCode == Domain.Enums.StatusCode.AccountNotFound ||
                result.Data.email != HttpContext.User.Identity.Name)
            {
                return Forbid();
            }
            
            var response = await _accountService.DeleteUser(accountId);
            
            if (!response.Data)
            {
                return BadRequest(result.Description);
            }

            return Ok();
        }
        
        #endregion
        
        
    }
}
