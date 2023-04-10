using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChip_API.Domain.DTO.Area;
using DripChip_API.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("areas")]
    [ApiController]
    [Authorize]
    public class AreasController : ControllerBase
    {
        [HttpGet("{areaId:long?}")]
        public async Task<ActionResult> GetAreas(long areaId)
        {
            if (areaId <= 0)
            {
                return BadRequest("Неправильные входные данные");
            }


            return Ok();
        }        
        
        [HttpPost]
        public async Task<ActionResult> AddArea([FromBody] DTOArпше ea area)
        {


            return Ok(area);
        }
    }
}
