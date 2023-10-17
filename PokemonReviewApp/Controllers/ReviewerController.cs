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
    public class ReviewerController : ControllerBase
    {
        private IReviewerRepo reviewerRepo;
        private IMapper mapper;

        public ReviewerController(IReviewerRepo _reviewerRepo, IMapper _mapper)
        {
            reviewerRepo = _reviewerRepo;
            mapper = _mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Reviewer>))]
        public IActionResult GetAll()
        {
            var reviewers = mapper.Map<List<ReviewerDTO>>(reviewerRepo.GetAll());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult Get(int id)
        {
            bool reviewerExst = reviewerRepo.Exst(id);

            if (reviewerExst == false) { return NotFound(); }

            var reviewer = mapper.Map<ReviewerDTO>(reviewerRepo.Get(id));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviewer);
        }

        [HttpGet("name/{reviewerName}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetByName(string reviewerName)
        {
            var reviewer = mapper.Map<ReviewerDTO>(reviewerRepo.GetByName(reviewerName));

            if (!ModelState.IsValid) return BadRequest(ModelState);
            return Ok(reviewer);
        }

        [HttpPost("create")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewer = reviewerRepo.GetByName(reviewerDTO.FirstName+reviewerDTO.LastName);

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer Already Exists!");
                return StatusCode(422, ModelState);
            }

            var reviewerMap = mapper.Map<Reviewer>(reviewerDTO);

            reviewerRepo.Create(reviewerMap);

            return Ok("Successfully Create");
        }

        [HttpPut("update/{reviewerId}")]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerDTO)
        {
            if (reviewerDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (reviewerDTO.Id != reviewerId)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reviewerRepo.Exst(reviewerId) == false)
            {
                return NotFound();
            }

            var reviewerMap  = mapper.Map<Reviewer>(reviewerDTO);
            reviewerRepo.Update(reviewerMap);

            return Ok("Successfully Updated");
        }

        [HttpDelete("{reviewerId}")]
        public IActionResult deleteReviewer(int reviewerId)
        {
            if (reviewerRepo.Exst(reviewerId) == false)
            {
                return NotFound();
            }

            reviewerRepo.Delete(reviewerId);
            return Ok("Successfully Deleted");
        }
    }
}
