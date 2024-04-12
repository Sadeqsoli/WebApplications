using Microsoft.EntityFrameworkCore;
using PokemonApp.Data;
using PokemonApp.Interfaces;
using PokemonApp.Models;
using System.Security.Cryptography;

namespace PokemonApp.Repositories
{
	public class CountryRepository : ICountryRepository
	{
		readonly DataContext _context;
		public CountryRepository(DataContext context)
		{
			_context = context;

		}
		public bool CountryExists(int cId)
		{
			return _context.Countries.Any(c =>  c.Id == cId);
		}


		public ICollection<Country> GetCountries()
		{
			return _context.Countries.OrderBy(c => c.Id).ToList();
		}

		public Country GetCountry(int cId)
		{
			return _context.Countries.Where(c => c.Id == cId).FirstOrDefault();
		}

		public Country GetCountry(string name)
		{
			return _context.Countries.Where(c => c.Name == name).FirstOrDefault();
		}

		public ICollection<Owner> GetOwnersFromCountry(int cId)
		{
			return _context.Owners.Where(o => o.Country.Id == cId).ToList();
		}

		public bool CreateCountry(Country country)
		{
			//change tracker
			_context.Add(country);

			return SaveCountry();
		}
		public bool SaveCountry()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		Country ICountryRepository.GetCountryByOwner(int ownerId)
		{
			return _context.Owners.Where(o => o.Id == ownerId).Select(o => o.Country).FirstOrDefault();
		}

		public bool UpdateCountry(Country country)
		{
			_context.Update(country);
			return SaveCountry();
		}
		public bool DeleteCountry(Country country)
		{
			_context.Remove(country);
			return SaveCountry();
		}
	}
}
