using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.DTO;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Repositories;
using System.Collections;

namespace PokemonApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PokemonController : Controller
	{
		private readonly IPokemonRepository _pokemonRepository;
		private readonly IReviewRepository _reviewRepository;
		private readonly IMapper _mapper;

		public PokemonController(IPokemonRepository pokemonRepository,
			IReviewRepository reviewRepository,
			IMapper mapper)
		{
			_pokemonRepository = pokemonRepository;
			_reviewRepository = reviewRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
		public IActionResult GetPokemons()
		{
			var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetPokemons());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemons);
		}


		[HttpGet("{pokeId}")]
		[ProducesResponseType(200, Type = typeof(Pokemon))]
		[ProducesResponseType(400)]
		public IActionResult GetPokemon(int pokeId)
		{
			if (!_pokemonRepository.PokemonExists(pokeId))
				return NotFound();

			var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemon(pokeId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemon);
		}

		[HttpGet("{pokeId}/rating")]
		[ProducesResponseType(200, Type = typeof(int))]
		[ProducesResponseType(400)]
		public IActionResult GetPokemonRating(int pokeId)
		{
			if (!_pokemonRepository.PokemonExists(pokeId))
				return NotFound();

			var pokemonRate = _pokemonRepository.GetPokemonRating(pokeId);

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemonRate);
		}


		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Pokemon))]
		[ProducesResponseType(400)]
		public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDTO pokemonCreate)
		{
			if (pokemonCreate == null)
				return BadRequest(ModelState);
			var pokemon = _pokemonRepository.GetPokemons()
				.Where(c => c.Name.Trim().ToUpper() == pokemonCreate.Name.Trim().ToUpper())
				.FirstOrDefault();

			if (pokemon != null)
			{
				ModelState.AddModelError("", "Pokemon Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);




			if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
			{
				ModelState.AddModelError("", "Failed to save the object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the pokemon:" + pokemonMap.Name);
		}



		[HttpPut("{pokeId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDTO pokemonUpdate)
		{
			if (pokemonUpdate == null)
				return BadRequest(ModelState);
			if (pokeId != pokemonUpdate.Id)
				return BadRequest(ModelState);

			if (!_pokemonRepository.PokemonExists(pokeId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

			if (!_pokemonRepository.UpdatePokemon(ownerId, catId,pokemonMap))
			{
				ModelState.AddModelError("", "Failed to Update the Pokemon!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the Pokemon to: " + pokemonMap.Name);
		}

		[HttpDelete("{pokeId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeletePokemon(int pokeId)
		{
			if (!_pokemonRepository.PokemonExists(pokeId))
				return NotFound();
			var pokemon = _pokemonRepository.GetPokemon(pokeId);
			var reviews = _reviewRepository.GetReviewsOfAPokemon(pokeId);
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			if (!_reviewRepository.DeleteReviews(reviews.ToList()))
			{
				ModelState.AddModelError("", "Failed to Delete the reviews!");
				return StatusCode(500, ModelState);
			}

			if (!_pokemonRepository.DeletePokemon(pokemon))
			{
				ModelState.AddModelError("", "Failed to Delete the pokemon!");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully Deleted the pokemon to: " + pokemon.Name);
		}


	}
}
