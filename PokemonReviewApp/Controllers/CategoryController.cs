using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.DTOs;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private ICategoryRepo categoryRepo;
        private IPokemonRepo pokemonRepo;
        private IMapper mapper;

        public CategoryController(ICategoryRepo _categoryRepo, IPokemonRepo _pokemonRepo, IMapper _mapper)
        {
            categoryRepo = _categoryRepo;
            pokemonRepo = _pokemonRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Category>))]
        public IActionResult GetAll()
        {
            var categories = mapper.Map<List<CategoryDTO>>(categoryRepo.GetAll());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult Get(int id)
        {
            bool categoryExst = categoryRepo.Exst(id);

            if (categoryExst == false)
            {
                return NotFound();
            }

            var category = mapper.Map<CategoryDTO>(categoryRepo.Get(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(category);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = categoryRepo.GetByName(categoryDTO.Name);

            if (category != null)
            {
                ModelState.AddModelError("", "Category Already Exists!");
                return StatusCode(422, ModelState);
            }

            var categoryMap = mapper.Map<Category>(categoryDTO);

            categoryRepo.Create(categoryMap);

            return Ok("Successfully Create");
        }

        [HttpPut("update/{categoryId}")]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDTO categoryDTO)
        {
            if (categoryDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (categoryDTO.Id != categoryId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoryRepo.Exst(categoryId) == false)
            {
                return NotFound();
            }

            var categoryMap = mapper.Map<Category>(categoryDTO);
            categoryRepo.Update(categoryMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{categoryId}")]
        public IActionResult deleteCategory(int categoryId) 
        {
            if (categoryRepo.Exst(categoryId) == false)
            {
                return NotFound();
            }

            if (pokemonRepo.GetPokemonByCategory(categoryId).Count > 0) 
            {
                ModelState.AddModelError("","There are pokemons associated with this category");
                return StatusCode(400, ModelState);
            }

            categoryRepo.Delete(categoryId);
            return Ok("Successfully Deleted");
        }
    }
}
