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
    public class ReviewController : ControllerBase
    {
        private IReviewRepo reviewRepo;
        private IReviewerRepo reviewerRepo;
        private IPokemonRepo pokemonRepo;
        private IMapper mapper;
        public ReviewController(IReviewRepo _reviewRepo, IReviewerRepo _reviewerRepo, IPokemonRepo _pokemonRepo, IMapper _mapper)
        {
            reviewRepo = _reviewRepo;
            reviewerRepo = _reviewerRepo;
            pokemonRepo = _pokemonRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        public IActionResult GetAll()
        {
            var reviews = mapper.Map<List<ReviewDTO>>(reviewRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult Get(int id)
        {
            bool reviewExst = reviewRepo.Exst(id);

            if (reviewExst == false) { return NotFound(); }

            var owner = mapper.Map<ReviewDTO>(reviewRepo.Get(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        public IActionResult GetReviewsOfAPokemon(int pokeId)
        {
            var reviews = mapper.Map<List<ReviewDTO>>(reviewRepo.GetReviewsOfAPokemon(pokeId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpGet("reviewer/{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(List<Review>))]
        public IActionResult GetReviewsByReviewer (int reviewerId)
        {
            var reviews = mapper.Map<List<ReviewDTO>>(reviewRepo.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviews);
        }

        [HttpPost("create")]
        public IActionResult createReview([FromQuery] string reviewerName, [FromQuery] string pokemonName, ReviewDTO reviewDTO) 
        {
            if (reviewDTO == null) 
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewerRepo.GetByName(reviewerName) == null) 
            {
                ModelState.AddModelError("","Reviewer Doesn't Exist!");
                return StatusCode(422, ModelState);
            }

            if (pokemonRepo.GetByName(pokemonName) == null)
            {
                ModelState.AddModelError("", "Pokemon Doesn't Exist!");
                return StatusCode(422, ModelState);

            }

            var reviewMap = mapper.Map<Review>(reviewDTO);
            reviewMap.Reviewer = reviewerRepo.GetByName(reviewerName);
            reviewMap.Pokemon = pokemonRepo.GetByName(pokemonName);
            reviewRepo.Create(reviewMap);

            return Ok("Successfully Create");
        }

        [HttpPut("update/{reviewId}")]
        public IActionResult UpdateReviewer(int reviewId, [FromBody] ReviewDTO reviewDTO)
        {
            if (reviewDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewDTO.Id != reviewId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewerRepo.Exst(reviewId) == false)
            {
                return NotFound();
            }

            var reviewMap = mapper.Map<Review>(reviewDTO);
            reviewRepo.Update(reviewMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{reviewId}")]
        public IActionResult deleteReview(int reviewId)
        {
            if (reviewRepo.Exst(reviewId) == false)
            {
                return NotFound();
            }

            reviewRepo.Delete(reviewId);
            return Ok("Successfully Deleted");
        }
    }
}
