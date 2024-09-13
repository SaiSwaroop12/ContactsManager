using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName {  get; set; }

        public override bool Equals(object? obj)
        {
           if(obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse response = (CountryResponse)obj;    

            return response.CountryId == CountryId && response.CountryName==CountryName;
        }

    }

    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            CountryResponse countryresponse = new CountryResponse() { CountryId = country.CountryId,CountryName=country.CountryName};

            return countryresponse;

        }
    }
}


