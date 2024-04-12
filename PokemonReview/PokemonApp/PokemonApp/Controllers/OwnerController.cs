using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonApp.DTO;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using PokemonApp.Repositories;

namespace PokemonApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OwnerController : Controller
	{
		private readonly IOwnerRepository _ownerRepository;
		private readonly ICountryRepository _countryRepository;

		private readonly IMapper _mapper;
		public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IMapper mapper)
		{
			_ownerRepository = ownerRepository;
			_countryRepository = countryRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
		public IActionResult GetOwners()
		{
			var owners = _mapper.Map<List<OwnerDTO>>(_ownerRepository.GetOwners());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(owners);
		}
		[HttpGet("{ownerId}")]
		[ProducesResponseType(200, Type = typeof(Owner))]
		public IActionResult GetOwner(int ownerId)
		{
			if (!_ownerRepository.OwnerExists(ownerId))
				return NotFound();
			var owner = _mapper.Map<OwnerDTO>(_ownerRepository.GetOwner(ownerId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(owner);
		}

		[HttpGet("{ownerId}/pokemon")]
		[ProducesResponseType(200, Type = typeof(Pokemon))]
		[ProducesResponseType(400)]
		public IActionResult GetPokemonByOwner(int ownerId)
		{
			if (!_ownerRepository.OwnerExists(ownerId))
				return NotFound();
			var pokemonList = _mapper.Map<List<PokemonDTO>>(_ownerRepository.GetPokemonsOfAnOwner(ownerId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemonList);
		}



		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Owner))]
		[ProducesResponseType(400)]
		public IActionResult CreateOwner([FromQuery] int countryId,[FromBody] OwnerDTO ownerCreate)
		{
			if (ownerCreate == null)
				return BadRequest(ModelState);

			var owner = _ownerRepository.GetOwners()
				.Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.Trim().ToUpper())
				.FirstOrDefault();

			if (owner != null)
			{
				ModelState.AddModelError("", "Owner Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var ownerMap = _mapper.Map<Owner>(ownerCreate);

			ownerMap.Country = _countryRepository.GetCountry(countryId);

			if (!_ownerRepository.CreateOwner(ownerMap))
			{
				ModelState.AddModelError("", "Failed to save the object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the owner: " + ownerMap.FirstName);
		}

		[HttpPut("{ownerId}")]
		[ProducesResponseType(400)]
		[ProducesResponseType(204)]
		[ProducesResponseType(404)]
		public IActionResult UpdateOwner([FromQuery] int ownerId, [FromBody] OwnerDTO ownerUpdate)
		{
			if (ownerUpdate == null)
				return BadRequest(ModelState);
			if (ownerId != ownerUpdate.Id)
				return BadRequest(ModelState);

			if (!_ownerRepository.OwnerExists(ownerId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var ownerMap = _mapper.Map<Owner>(ownerUpdate);

			if (!_ownerRepository.UpdateOwner(ownerMap))
			{
				ModelState.AddModelError("", "Failed to Update the owner!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the owner to: " + ownerMap.FirstName);
		}

		[HttpDelete("{ownerId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeleteOwne(int ownerId)
		{
			if (!_ownerRepository.OwnerExists(ownerId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var owner = _ownerRepository.GetOwner(ownerId);

			if (!_ownerRepository.DeleteOwner(owner))
			{
				ModelState.AddModelError("", "Failed to Delete the owner!");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully Deleted the owner to: " + owner.FirstName);
		}


	}
}
