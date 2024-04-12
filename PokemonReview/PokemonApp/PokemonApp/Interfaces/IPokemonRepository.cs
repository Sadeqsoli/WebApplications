using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
	public interface IPokemonRepository
	{
		ICollection<Pokemon> GetPokemons();
		Pokemon GetPokemon(int pokeId);
		Pokemon GetPokemon(string name);
		int GetPokemonRating(int pokId);
		bool PokemonExists(int pokId);
		bool CreatePokemon(int ownerId, int catId, Pokemon pokemon);
		bool UpdatePokemon(int ownerId, int catId, Pokemon pokemon);
		bool DeletePokemon(Pokemon pokemon);
		bool SavePokemon();
	}
}
