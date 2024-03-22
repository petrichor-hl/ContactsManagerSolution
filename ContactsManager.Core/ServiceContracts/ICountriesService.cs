using Entities;
using ContactsManager.Core.DTOs;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country entity
    /// </summary>
    public interface ICountriesService
    {
        /// <summary>
        /// Adds a country object to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Return the country object after adding it (including newly generated Id)</returns>
        public Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        public Task<List<CountryResponse>> GetAllCountries();

        public Task<CountryResponse?> GetCountryByCountryId(Guid? countryId);
    }
}
