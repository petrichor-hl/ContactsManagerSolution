﻿using Entities;

namespace RepositoryContracts
{
	public interface ICountriesRepository
	{
		Task<Country> AddCountry(Country country);

		Task<List<Country>> GetAllCountries();

		Task<Country?> GetCountryByCountryID(Guid countryID);

		Task<Country?> GetCountryByCountryName(string countryName);
	}
}
