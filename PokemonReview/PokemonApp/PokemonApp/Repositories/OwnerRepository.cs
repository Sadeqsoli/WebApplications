using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;

namespace PokemonApp.Repositories
{
	public class OwnerRepository : IOwnerRepository
	{
		readonly DataContext _context;
        public OwnerRepository(DataContext context)
		{
			_context = context;
		}

		public Owner GetOwner(int ownerId)
		{
			return _context.Owners.Where(o => o.Id == ownerId).FirstOrDefault();
		}

		public ICollection<Owner> GetOwners()
		{
			return _context.Owners.OrderBy(o => o.Id).ToList();
		}

		public ICollection<Owner> GetOwnersOfAPokemon(int pokeId)
		{
			return _context.PokemonOwners.Where(po => po.PokemonId == pokeId).Select(po => po.Owner).ToList();
		}

		public ICollection<Pokemon> GetPokemonsOfAnOwner(int ownerId)
		{
			return _context.PokemonOwners.Where(po => po.OwnerId == ownerId).Select(po => po.Pokemon).ToList();
		}

		public bool OwnerExists(int ownerId)
		{
			return _context.Owners.Any(o => o.Id == ownerId);
		}

		public bool CreateOwner(Owner owner)
		{
			//change tracker
			_context.Add(owner);
			return SaveOwner();
		}

		public bool SaveOwner()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool UpdateOwner(Owner owner)
		{
			_context.Update(owner);
			return SaveOwner();
		}
		public bool DeleteOwner(Owner owner)
		{
			_context.Remove(owner);
			return SaveOwner();
		}
	}
}
