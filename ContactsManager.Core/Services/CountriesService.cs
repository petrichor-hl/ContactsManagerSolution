using Entities;
using RepositoryContracts;
using ServiceContracts;
using ContactsManager.Core.DTOs;

namespace Services
{
    public class CountriesService : ICountriesService
    {
		private readonly ICountriesRepository _countriesRepository;

		public CountriesService(ICountriesRepository countriesRepository)
        {
			_countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists");
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryId = Guid.NewGuid();
			await _countriesRepository.AddCountry(country);

			return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
			List<Country> countries = await _countriesRepository.GetAllCountries();
			return countries
			  .Select(country => country.ToCountryResponse()).ToList();
		}

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }

            Country? country = await _countriesRepository.GetCountryByCountryID(countryId.Value);

			return country?.ToCountryResponse();
            // return null if country == null
            // else return country.ToCountryResponse();
        }
    }
}
