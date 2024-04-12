using PokemonApp.Models;

namespace PokemonApp.Interfaces
{
	public interface ICountryRepository
	{
		ICollection<Country> GetCountries();
		Country GetCountry(int cId);
		Country GetCountryByOwner(int ownerId);
		ICollection<Owner> GetOwnersFromCountry(int cId);
		Country GetCountry(string name);
		bool CountryExists(int cId);
		bool CreateCountry(Country country);
		bool UpdateCountry(Country country);
		bool DeleteCountry(Country country);
		bool SaveCountry();
	}
}
