using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repos;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private IOwnerRepo ownerRepo;
        private ICountryRepo countryRepo;
        private IPokemonRepo pokemonRepo;
        private IMapper mapper;
        public OwnerController(IOwnerRepo _ownerRepo, ICountryRepo _countryRepo, IPokemonRepo _pokemonRepo ,IMapper _mapper)
        {
            ownerRepo = _ownerRepo;
            countryRepo = _countryRepo;
            pokemonRepo = _pokemonRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        public IActionResult GetAll()
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owners);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult Get(int id)
        {
            bool ownerExst = ownerRepo.Exst(id);

            if (ownerExst == false) { return NotFound(); }

            var owner = mapper.Map<OwnerDTO>(ownerRepo.Get(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("name/{ownerName}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetByName(string ownerName)
        {
            var owner = mapper.Map<OwnerDTO>(ownerRepo.GetByName(ownerName));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("country/{countryId}")]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        public IActionResult GetOwnersByCountry(int countryId)
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepo.GetOwnersByCountry(countryId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(List<Owner>))]
        public IActionResult GetOwnersByPokemon(int pokeId)
        {
            var owners = mapper.Map<List<OwnerDTO>>(ownerRepo.GetOwnersByPokemon(pokeId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owners);
        }

        [HttpPost("create")]
        public IActionResult createOwner([FromQuery] string countryName,[FromBody] OwnerDTO ownerDTO) 
        {
            if (ownerDTO == null) 
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = countryRepo.GetByName(countryName);

            if (data == null)
            {
                ModelState.AddModelError("", "Country Doesn't Exist!");
                return StatusCode(422, ModelState);
            }

            var Data = ownerRepo.GetByName(ownerDTO.Name);

            if (Data != null) 
            {
                ModelState.AddModelError("","Ower Already Exists!");
                return StatusCode(422,ModelState);
            }

            var ownerMap = mapper.Map<Owner>(ownerDTO);
            ownerMap.Country = countryRepo.GetByName(countryName);
            ownerRepo.Create(ownerMap);

            return Ok("Successfully Created");
        }  
        
        [HttpPost("ownPokemon")]
        public IActionResult ownPokemon([FromQuery] string ownerName,[FromQuery] string pokemonName) 
        {
            if (ownerName == null && pokemonName == null) 
            {
                return BadRequest(ModelState);
            }

            var Data = ownerRepo.GetByName(ownerName);

            if (Data == null) 
            {
                ModelState.AddModelError("","Ower Doesn't Exists!");
                return StatusCode(422,ModelState);
            }

            var data = pokemonRepo.GetByName(pokemonName);

            if (data == null)
            {
                ModelState.AddModelError("", "Pokemon Doesn't Exists!");
                return StatusCode(422, ModelState);
            }

            var data1 = ownerRepo.AlreadyOwnedPokemon(ownerName, pokemonName); ;

            if (data1 != null)
            {
                ModelState.AddModelError("", "Owner Already Owns This Pokemon!");
                return StatusCode(422, ModelState);
            }

            ownerRepo.OwnPokemon(ownerName, pokemonName);

            return Ok("Successfully Created");
        }

        [HttpPut("update/{ownerId}")]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerDTO)
        {
            if (ownerDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (ownerDTO.Id != ownerId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (countryRepo.Exst(ownerId) == false)
            {
                return NotFound();
            }

            var ownerMap = mapper.Map<Owner>(ownerDTO);
            ownerRepo.Update(ownerMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{ownerId}")]
        public IActionResult deleteOwner(int ownerId)
        {
            if (ownerRepo.Exst(ownerId) == false)
            {
                return NotFound();
            }

            ownerRepo.Delete(ownerId);
            return Ok("Successfully Deleted");
        }
    }
}
