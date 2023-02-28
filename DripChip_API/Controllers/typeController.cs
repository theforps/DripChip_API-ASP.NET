using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class typeController : ControllerBase
    {
        [HttpGet("/animals/types")]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
