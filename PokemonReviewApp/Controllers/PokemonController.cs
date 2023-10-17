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
    public class PokemonController : ControllerBase
    {
        private IPokemonRepo pokemonRepo;
        private ICategoryRepo categoryRepo;
        private IMapper mapper;

        public PokemonController(IPokemonRepo _pokemonRepo, ICategoryRepo _categoryRepo, IMapper _mapper) 
        {
            pokemonRepo = _pokemonRepo;
            categoryRepo = _categoryRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
        public IActionResult GetAll() 
        {
            var pokemons = mapper.Map<List<PokemonDTO>>(pokemonRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        public IActionResult Get(int id) 
        {
            bool pokemonExst = pokemonRepo.Exst(id);

            if (pokemonExst == false) { return NotFound();}

            var pokemon = mapper.Map<PokemonDTO>(pokemonRepo.Get(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
        public IActionResult GetPokemonByCategory(int categoryId) 
        {
            var pokemons = mapper.Map<List<PokemonDTO>>(pokemonRepo.GetPokemonByCategory(categoryId));

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(List<Pokemon>))]
        public IActionResult GetPokemonByOwner(int ownerId)
        {
            var pokemons = mapper.Map<List<PokemonDTO>>(pokemonRepo.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpPost("create")]
        public IActionResult createPokemon([FromQuery] string categoryName, [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = categoryRepo.GetByName(categoryName);

            if (data == null)
            {
                ModelState.AddModelError("", "Category Doesn't Exist!");
                return StatusCode(422, ModelState);
            }

            var Data = pokemonRepo.GetByName(pokemonDTO.Name);

            if (Data != null)
            {
                ModelState.AddModelError("", "Pokemon Already Exists!");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = mapper.Map<Pokemon>(pokemonDTO);
            pokemonRepo.Create(pokemonMap, categoryName);

            return Ok("Successfully Created");
        }

        [HttpPut("update/{pokemonId}")]
        public IActionResult UpdatePokemon(int pokemonId, [FromBody] PokemonDTO pokemonDTO)
        {
            if (pokemonDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (pokemonDTO.Id != pokemonId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pokemonRepo.Exst(pokemonId) == false)
            {
                return NotFound();
            }

            var pokemonMap = mapper.Map<Pokemon>(pokemonDTO);
            pokemonRepo.Update(pokemonMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{pokemonId}")]
        public IActionResult deletePokemon(int pokemonId)
        {
            if (pokemonRepo.Exst(pokemonId) == false)
            {
                return NotFound();
            }

            pokemonRepo.Delete(pokemonId);
            return Ok("Successfully Deleted");
        }
    }
}
