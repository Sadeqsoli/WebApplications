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
	public class CountryController : Controller
	{
		private readonly ICountryRepository _countryRepository;
		private readonly IMapper _mapper;

		public CountryController(ICountryRepository countryRepository, IMapper mapper)
		{
			_countryRepository = countryRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
		public IActionResult GetCountries()
		{
			var countries = _mapper.Map<List<CountryDTO>>(_countryRepository.GetCountries());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(countries);
		}


		[HttpGet("{cId}")]
		[ProducesResponseType(200, Type = typeof(Country))]
		[ProducesResponseType(400)]
		public IActionResult GetCountry(int cId)
		{
			if (!_countryRepository.CountryExists(cId))
				return NotFound();

			var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountry(cId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(country);
		}

		[HttpGet("owners/{ownerId}")]
		[ProducesResponseType(200, Type = typeof(Country))]
		[ProducesResponseType(400)]
		public IActionResult GetCountryByOwner(int ownerId)
		{
			if (!_countryRepository.CountryExists(ownerId))
				return NotFound();

			var country = _mapper.Map<CountryDTO>(_countryRepository.GetCountryByOwner(ownerId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(country);
		}

		[HttpGet("countries/{cId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
		[ProducesResponseType(400)]
		public IActionResult GetOwnersFromCountry(int cId)
		{
			if (!_countryRepository.CountryExists(cId))
				return NotFound();

			var country = _mapper.Map<List<OwnerDTO>>(_countryRepository.GetOwnersFromCountry(cId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(country);
		}



		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Country))]
		[ProducesResponseType(400)]
		public IActionResult CreateCountry([FromBody] CountryDTO countryCreate)
		{
			if (countryCreate == null)
				return BadRequest(ModelState);
			var Country = _countryRepository.GetCountries()
				.Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.Trim().ToUpper())
				.FirstOrDefault();

			if (Country != null)
			{
				ModelState.AddModelError("", "Country Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var countryMap = _mapper.Map<Country>(countryCreate);

			if (!_countryRepository.CreateCountry(countryMap))
			{
				ModelState.AddModelError("", "Failed to save object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the country:" + countryMap.Name);
		}


		[HttpPut("{countryId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateCountry([FromQuery] int countryId, [FromBody] CountryDTO countryUpdate)
		{
			if (countryUpdate == null)
				return BadRequest(ModelState);
			if (countryId != countryUpdate.Id)
				return BadRequest(ModelState);

			if (!_countryRepository.CountryExists(countryId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var countryMap = _mapper.Map<Country>(countryUpdate);

			if (!_countryRepository.UpdateCountry(countryMap))
			{
				ModelState.AddModelError("", "Failed to Update the country!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the country to: " + countryMap.Name);
		}
		[HttpDelete("{countryId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeleteCountry(int countryId)
		{
			if (!_countryRepository.CountryExists(countryId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var country = _countryRepository.GetCountry(countryId);

			if (!_countryRepository.DeleteCountry(country))
			{
				ModelState.AddModelError("", "Failed to Delete the country!");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully Deleted the country to: " + country.Name);
		}
	}
}
