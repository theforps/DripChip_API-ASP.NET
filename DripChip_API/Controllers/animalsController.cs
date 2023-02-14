using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DripChip_API.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DripChip_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class animalsController : ControllerBase
    {
        private readonly IAnimalService _animalService;

        public animalsController(IAnimalService animalService)
        {
            _animalService = animalService;
        }
        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{animalId:int}")]
        public async Task<ActionResult> GetAnimalById(int animalId)
        {
            if (animalId == null || animalId == 0)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            
            var animal = await _animalService.GetAnimal(animalId);

            if (animal.StatusCode == Domain.Enums.StatusCode.AnimalNotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            
            return Ok(animal.Data);
        }
        
        [HttpGet("search")]
        public ActionResult GetAnimal()
        {
            return Ok();
        }
        
        /*
        [HttpGet("{typeId:int}",Name = "types")]
        public ActionResult GetAnimalType(int typeId)
        {
            return Ok();
        }
        */
    }
}
