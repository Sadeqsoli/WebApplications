using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.DTO;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Repositories;
using System.Security.Cryptography;

namespace PokemonApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewController : Controller
	{
		private readonly IReviewRepository _reviewRepository;
		private readonly IReviewerRepository _reviewerRepository;
		private readonly IPokemonRepository _pokemonRepository;
		private readonly IMapper _mapper;

		public ReviewController(IReviewRepository reviewRepository, IReviewerRepository reviewerRepository,
			IPokemonRepository pokemonRepository, IMapper mapper)
		{
			_reviewRepository = reviewRepository;
			_reviewerRepository = reviewerRepository;
			_pokemonRepository = pokemonRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
		public IActionResult GetReviews()
		{
			var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(reviews);
		}
		[HttpGet("{reviewId}")]
		[ProducesResponseType(200, Type = typeof(Review))]
		public IActionResult GetReview(int reviewId)
		{
			if (!_reviewRepository.ReviewExists(reviewId))
				return NotFound();

			var review = _mapper.Map<ReviewDTO>(_reviewRepository.GetReview(reviewId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(review);
		}

		[HttpGet("pokemon/{pokeId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
		[ProducesResponseType(400)]
		public IActionResult GetReviewForAPokemon(int pokeId)
		{
			var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsOfAPokemon(pokeId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(reviews);

		}


		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Review))]
		[ProducesResponseType(400)]
		public IActionResult CreateReview([FromQuery] int reviewerId, [FromQuery] int pokeId, [FromBody] ReviewDTO reviewCreate)
		{
			if (reviewCreate == null)
				return BadRequest(ModelState);
			var review = _reviewRepository.GetReviews()
				.Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.Trim().ToUpper())
				.FirstOrDefault();

			if (review != null)
			{
				ModelState.AddModelError("", "Review Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var reviewMap = _mapper.Map<Review>(reviewCreate);

			reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);
			reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);

			if (!_reviewRepository.CreateReview(reviewMap))
			{
				ModelState.AddModelError("", "Failed to save the object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the Review: " + reviewMap.Title);
		}


		[HttpPut("{reviewId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDTO reviewUpdate)
		{
			if (reviewUpdate == null)
				return BadRequest(ModelState);
			if (reviewId != reviewUpdate.Id)
				return BadRequest(ModelState);

			if (!_reviewRepository.ReviewExists(reviewId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var reviewMap = _mapper.Map<Review>(reviewUpdate);

			if (!_reviewRepository.UpdateReview(reviewMap))
			{
				ModelState.AddModelError("", "Failed to Update the Review!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the Review to: " + reviewMap.Title);
		}


		[HttpDelete("{reviewId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeleteReview(int reviewId)
		{
			if (!_reviewRepository.ReviewExists(reviewId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var review = _reviewRepository.GetReview(reviewId);

			if (!_reviewRepository.DeleteReview(review))
			{
				ModelState.AddModelError("", "Failed to Update the Review!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Deleted the Review to: " + review.Title);
		}

	}
}
