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
    public class CountryController : ControllerBase
    {
        private ICountryRepo countryRepo;
        private IOwnerRepo ownerRepo;
        private IMapper mapper;
        public CountryController(ICountryRepo _countryRepo, IOwnerRepo _ownerRepo, IMapper _mapper)
        {
            countryRepo = _countryRepo;
            ownerRepo = _ownerRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Country>))]
        public IActionResult GetAll() 
        {
            var pokemons = mapper.Map<List<CountryDTO>>(countryRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult Get(int id)
        {
            bool countryExst = countryRepo.Exst(id);

            if (countryExst == false)
            {
                return NotFound();
            }

            var country = mapper.Map<CountryDTO>(countryRepo.Get(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpGet("owner/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = mapper.Map<CountryDTO>(countryRepo.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(country);
        }

        [HttpPost("create")]
        public IActionResult createCountry([FromBody] CountryDTO countryDTO) 
        {
            if (countryDTO == null) 
            { 
                return BadRequest(ModelState); 
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var Data = countryRepo.GetByName(countryDTO.Name);

            if (Data != null) 
            {
                ModelState.AddModelError("","Country Alreday Exists!");
                return StatusCode(422,ModelState);
            }

            var countryMap = mapper.Map<Country>(countryDTO);
            countryRepo.Create(countryMap);

            return Ok("Successfully Created");
        }

        [HttpPut("update/{countryId}")]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDTO countryDTO)
        {
            if (countryDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (countryDTO.Id != countryId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (countryRepo.Exst(countryId) == false)
            {
                return NotFound();
            }

            var countryMap = mapper.Map<Country>(countryDTO);
            countryRepo.Update(countryMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{countryId}")]
        public IActionResult deleteCountry(int countryId)
        {
            if (countryRepo.Exst(countryId) == false)
            {
                return NotFound();
            }

            if (ownerRepo.GetOwnersByCountry(countryId).Count > 0)
            {
                ModelState.AddModelError("", "There are owners associated with this category");
                return StatusCode(400, ModelState);
            }

            countryRepo.Delete(countryId);
            return Ok("Successfully Deleted");
        }
    }
}