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
    public class animalsController : ControllerBase
    {
        [HttpGet("{animalId:int}")]
        public ActionResult GetAnimalById(int animalId)
        {
            return Ok();
        }
        
        [HttpGet("search")]
        public ActionResult GetAnimal()
        {
            return Ok();
        }
        
        [HttpGet("{typeId:int}",Name = "types")]
        public ActionResult GetAnimalType(int typeId)
        {
            return Ok();
        }
    }
}
