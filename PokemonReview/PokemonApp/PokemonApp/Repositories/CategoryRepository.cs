using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
	public class CategoryRepository : ICategoryRepository
	{
		readonly DataContext _context;
		public CategoryRepository(DataContext context)
		{
			_context = context;

		}
		public bool CategoryExists(int catId)
		{
			return _context.Categories.Any(c => c.Id == catId);
		}


		public ICollection<Category> GetCategories()
		{
			return _context.Categories.OrderBy(c => c.Id).ToList();
		}

		public Category GetCategory(int catId)
		{
			return _context.Categories.Where(c => c.Id == catId).FirstOrDefault();
		}

		public Category GetCategory(string name)
		{
			return _context.Categories.Where(c => c.Name == name).FirstOrDefault();
		}

		public ICollection<Pokemon> GetPokemonByCategory(int catId)
		{
			return _context.PokemonCategories.Where(c => c.CategoryId == catId).Select(pc => pc.Pokemon).ToList();
		}

		public bool CreateCategory(Category category)
		{
			//change tracker
			_context.Add(category);

			return SaveCategory();
		}
		public bool SaveCategory()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateCategory(Category category)
		{
			_context.Update(category);
			return SaveCategory();
		}
		public bool DeleteCategory(Category category)
		{
			_context.Remove(category);
			return SaveCategory();
		}
	}
}
