using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
	public interface IOwnerRepository
	{
		ICollection<Owner> GetOwners();
		Owner GetOwner(int ownerId);
		ICollection<Owner> GetOwnersOfAPokemon(int pokeId);
		ICollection<Pokemon> GetPokemonsOfAnOwner(int ownerId);

		bool OwnerExists(int ownerId);

		bool CreateOwner(Owner owner);
		bool UpdateOwner(Owner owner);
		bool DeleteOwner(Owner owner);
		bool SaveOwner();
	}
}
