using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;

namespace PokemonApp.Repositories
{
	public class PokemonRepository : IPokemonRepository
	{
		readonly DataContext _context;
		public PokemonRepository(DataContext context)
		{
			_context = context;

		}


		public Pokemon GetPokemon(int pokeId)
		{
			return _context.Pokemons.Where(p => p.Id == pokeId).FirstOrDefault();
		}

		public Pokemon GetPokemon(string name)
		{
			return _context.Pokemons.Where(p => p.Name == name).FirstOrDefault();
		}

		public int GetPokemonRating(int pokId)
		{
			var reviews = _context.Reviews.Where(r => r.Pokemon.Id == pokId);

			var count = reviews.Count();
			if (count <= 0)
				return 0;

			var avgRating = (int)(reviews.Sum(r => r.Rating) / count);

			return avgRating;
		}

		public ICollection<Pokemon> GetPokemons()
		{
			return _context.Pokemons.OrderBy(p => p.Id).ToList();
		}

		public bool PokemonExists(int pokId)
		{
			return _context.Pokemons.Any(p => p.Id == pokId);
		}

		public bool CreatePokemon(int ownerId, int catId, Pokemon pokemon)
		{
			var pokemonOwnerEntity = _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
			var category = _context.Categories.Where(c => c.Id == catId).FirstOrDefault();

			var pokemonOwner = new PokemonOwner()
			{
				OwnerId = pokemonOwnerEntity.Id,
				Owner = pokemonOwnerEntity,
				Pokemon = pokemon,
				PokemonId = pokemon.Id
			};
			_context.Add(pokemonOwner);


			var pokemonCategory = new PokemonCategory()
			{
				CategoryId = category.Id,
				Category = category,
				Pokemon = pokemon,
				PokemonId = pokemon.Id
			};
			_context.Add(pokemonCategory);


			_context.Add(pokemon);

			return SavePokemon();
		}
		public bool SavePokemon()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdatePokemon(int ownerId, int catId, Pokemon pokemon)
		{
			_context.Update(pokemon);

			return SavePokemon();
		}

		public bool DeletePokemon(Pokemon pokemon)
		{
			_context.Remove(pokemon);

			return SavePokemon();
		}
	}
}
