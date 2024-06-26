﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.DTO;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Repositories;

namespace PokemonApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReviewerController : Controller
	{
		private readonly IReviewerRepository _reviewerRepository;
		private readonly IMapper _mapper;

		public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
		{
			_reviewerRepository = reviewerRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
		public IActionResult GetReviewers()
		{
			var reviewers = _mapper.Map<List<ReviewerDTO>>(_reviewerRepository.GetReviewers());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(reviewers);
		}
		[HttpGet("{reviewerId}")]
		[ProducesResponseType(200, Type = typeof(Reviewer))]
		public IActionResult GetReviewer(int reviewerId)
		{
			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound();

			var reviewer = _mapper.Map<ReviewerDTO>(_reviewerRepository.GetReviewer(reviewerId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(reviewer);
		}

		[HttpGet("{reviewerId}/reviews")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
		[ProducesResponseType(400)]
		public IActionResult GetReviewsOfAReviewer(int reviewerId)
		{
			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound();

			var reviews = _mapper.Map<List<ReviewDTO>>(_reviewerRepository.GetReviewsOfAReviewer(reviewerId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(reviews);
		}

		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Reviewer))]
		[ProducesResponseType(400)]
		public IActionResult CreateReviewer([FromBody] ReviewerDTO reviewerCreate)
		{
			if (reviewerCreate == null)
				return BadRequest(ModelState);
			var reviewer = _reviewerRepository.GetReviewers()
				.Where(c => c.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.Trim().ToUpper())
				.FirstOrDefault();

			if (reviewer != null)
			{
				ModelState.AddModelError("", "Reviewer Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);

			if (!_reviewerRepository.CreateReviewer(reviewerMap))
			{
				ModelState.AddModelError("", "Failed to save object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the reviewer:" + reviewerMap.FirstName);
		}


		[HttpPut("{reviewerId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDTO reviewerUpdate)
		{
			if (reviewerUpdate == null)
				return BadRequest(ModelState);
			if (reviewerId != reviewerUpdate.Id)
				return BadRequest(ModelState);

			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);

			if (!_reviewerRepository.UpdateReviewer(reviewerMap))
			{
				ModelState.AddModelError("", "Failed to Update the Reviewer!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the Reviewer to: " + reviewerMap.FirstName);
		}
		[HttpDelete("{reviewerId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeleteReviewer(int reviewerId)
		{
			if (!_reviewerRepository.ReviewerExists(reviewerId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var reviewer = _reviewerRepository.GetReviewer(reviewerId);

			if (!_reviewerRepository.DeleteReviewer(reviewer))
			{
				ModelState.AddModelError("", "Failed to Delete the reviewer!");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully Deleted the reviewer to: " + reviewer.FirstName);
		}

	}
}
