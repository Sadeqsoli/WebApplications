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
	public class CategoryController : Controller
	{
		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
		{
			_categoryRepository = categoryRepository;
			_mapper = mapper;
		}


		[HttpGet]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
		public IActionResult GetCategory()
		{
			var categories = _mapper.Map<List<CategoryDTO>>(_categoryRepository.GetCategories());

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(categories);
		}


		[HttpGet("{catId}")]
		[ProducesResponseType(200, Type = typeof(Category))]
		[ProducesResponseType(400)]
		public IActionResult GetCategory(int catId)
		{
			if (!_categoryRepository.CategoryExists(catId))
				return NotFound();

			var category = _mapper.Map<CategoryDTO>(_categoryRepository.GetCategory(catId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(category);
		}
		[HttpGet("pokemon/{catId}")]
		[ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
		[ProducesResponseType(400)]
		public IActionResult GetPokemonByCategory(int catId)
		{
			if (!_categoryRepository.CategoryExists(catId))
				return NotFound();

			var pokemons = _mapper.Map<List<PokemonDTO>>(_categoryRepository.GetPokemonByCategory(catId));

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			return Ok(pokemons);
		}


		[HttpPost]
		[ProducesResponseType(204, Type = typeof(Category))]
		[ProducesResponseType(400)]
		public IActionResult CreateCategory([FromBody] CategoryDTO categoryCreate)
		{
			if (categoryCreate == null)
				return BadRequest(ModelState);
			var category = _categoryRepository.GetCategories()
				.Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.Trim().ToUpper())
				.FirstOrDefault();

			if (category != null)
			{
				ModelState.AddModelError("", "Category Already Exists!");
				return StatusCode(422, ModelState);
			}

			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var categoryMap = _mapper.Map<Category>(categoryCreate);

			if (!_categoryRepository.CreateCategory(categoryMap))
			{
				ModelState.AddModelError("", "Failed to save the object!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully created the category:" + categoryMap.Name);
		}

		[HttpPut("{catId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult UpdateCategory(int catId, [FromBody] CategoryDTO categoryUpdate)
		{
			if (categoryUpdate == null)
				return BadRequest(ModelState);
			if (catId != categoryUpdate.Id)
				return BadRequest(ModelState);

			if (!_categoryRepository.CategoryExists(catId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var categoryMap = _mapper.Map<Category>(categoryUpdate);

			if (!_categoryRepository.UpdateCategory(categoryMap))
			{
				ModelState.AddModelError("", "Failed to Update the Category!");
				return StatusCode(500, ModelState);
			}


			return Ok("Successfully Updated the category to: " + categoryMap.Name);
		}

		[HttpDelete("{categoryId}")]
		[ProducesResponseType(204)]
		[ProducesResponseType(400)]
		[ProducesResponseType(404)]
		public IActionResult DeleteCategory(int categoryId)
		{
			if (!_categoryRepository.CategoryExists(categoryId))
				return NotFound();

			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			var category = _categoryRepository.GetCategory(categoryId);

			if (!_categoryRepository.DeleteCategory(category))
			{
				ModelState.AddModelError("", "Failed to Update the Category!");
				return StatusCode(500, ModelState);
			}

			return Ok("Successfully Deleted the Category to: " + category.Name);
		}


	}
}
