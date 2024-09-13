using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
namespace Services
{
    public class CountriesService : ICountriesService
    {

        private List<Country> _countries;


        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //CountryAddrequest null argument exception
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //CountryName argument exception
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Duplicate argument exception
            if(_countries.Where(temp =>
            
               temp.CountryName == countryAddRequest.CountryName
            ).Count() > 0)
            {
                throw new ArgumentException("Duplicate countries canoot exist");
            };

            //Convert into country object
            Country country = countryAddRequest.ToCountry();

            //Generate a id 
            country.CountryId = Guid.NewGuid();

            //Add into list
            _countries.Add(country);

            //convert into country response
            CountryResponse countryResponse = country.ToCountryResponse();

            return countryResponse;




           
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select((temp) =>
                temp.ToCountryResponse()
            ).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            //Check null condition
            if(countryId == null)
            {
                return null;
            }


            //Fetch the country object by country id
          Country? country_from_list =   _countries.FirstOrDefault((temp) =>  temp.CountryId == countryId);


            //Convert into country response object

          if(country_from_list == null)
            {
                return null;
            }

          return country_from_list.ToCountryResponse();




        }
    }
}
