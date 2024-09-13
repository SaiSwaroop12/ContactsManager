using ServiceContracts.DTO;
namespace ServiceContracts
{
    public interface ICountriesService
    {
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

        public List<CountryResponse> GetAllCountries();

        public CountryResponse? GetCountryByCountryId(Guid? countryId);


    }
}
