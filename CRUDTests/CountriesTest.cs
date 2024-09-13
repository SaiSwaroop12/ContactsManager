using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;


namespace CRUDTests
{
  
    public class CountriesTest
    {

        public readonly ICountriesService _countriesService;

        
        public CountriesTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);

            });
        }


        [Fact]

        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);

            });

        }

        [Fact]
        public void AddCountry_CountryNameIsDuplicate()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "USA" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });


        }


          
        [Fact]

        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countryResponseList = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Contains(response, countryResponseList);






        }

        #endregion



        #region GetAllCountries
        [Fact]

        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> allCountries = _countriesService.GetAllCountries();

            Assert.Empty(allCountries);
        }

        [Fact]

        public void GetAllCountries_ProperDetails()
        {
            List<CountryAddRequest> countries_list_to_be_add = new List<CountryAddRequest>()
            {
            
                new CountryAddRequest(){ CountryName = "USA"},
                new CountryAddRequest(){CountryName="Japan"}
            };

            List<CountryResponse> country_response_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest countryAddRequest in countries_list_to_be_add)
            {
                country_response_list_from_add_country.Add(_countriesService.AddCountry(countryAddRequest));
            }

            List<CountryResponse> actualcountryResponseList = _countriesService.GetAllCountries();

            foreach(CountryResponse expectedCountry in country_response_list_from_add_country)
            {
                Assert.Contains(expectedCountry, actualcountryResponseList);
            }


        }
        #endregion


        #region GetCountryByCountryID

        [Fact]

        public void GetCountryByCountryID_nullCountryID()
        {
            Guid? countryId = null;

            CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(countryId);

            Assert.Null(countryResponse);
        }

        [Fact]

        public void GetCountryByCountryID_ProperCountryID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA"
            };
           CountryResponse countryResponse =  _countriesService.AddCountry(countryAddRequest);


            //Act
            CountryResponse? expectedCountryResponse = _countriesService.GetCountryByCountryId(countryResponse.CountryId);

            //Assert
            Assert.Equal(expectedCountryResponse, countryResponse);



        }

        #endregion


    }
}
