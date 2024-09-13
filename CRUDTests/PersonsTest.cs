using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ServiceContracts.Enums;
using Entities;

namespace CRUDTests
{
    public class PersonsTest
    {
        private readonly PersonsService _personsService;
        private readonly CountriesService _countriesService;

        public PersonsTest()
        {
            _personsService = new PersonsService();
            _countriesService = new CountriesService();
        }



        #region AddPerson

        [Fact]

        public void AddPerson_NullPersonRequest()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;


            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            //Act
            _personsService.AddPerson(personAddRequest));
        }

        [Fact]

        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };


            //Assert
            Assert.Throws<ArgumentException>(() =>
            //Act
            _personsService.AddPerson(personAddRequest));
        }


        [Fact]

        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Swaroop",
                Email = "lol",
                DateOfBirth = DateTime.Parse("2001-07-05"),
                Gender = GenderOptions.Male,
                CountryID = Guid.NewGuid(),
                ReceiveNewsLetters = true,
                Address = "abcd"
            };

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> personResponseList = _personsService.GetAllPersons();

            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, personResponseList);
        }

        #endregion


        #region GetPersonByPersonID

        [Fact]

        public void GetPersonByPersonID_NullID()
        {
            Guid? guid = null;

            Assert.Null(_personsService.GetPersonByPersonID(guid));
        }

        [Fact]

        public void GetPersonByPersonID_ProperID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "USA" };

            Guid countryID = _countriesService.AddCountry(countryAddRequest).CountryId;

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "lol",
                Email = "lol@h.com",
                Address = "ashdd",
                CountryID = countryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-05-07"),
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse_from_add = _personsService.AddPerson(personAddRequest);
            PersonResponse? personResponse_from_get = _personsService.GetPersonByPersonID(personResponse_from_add.PersonId);


            Assert.Equal(personResponse_from_add, personResponse_from_get);


        }

        #endregion

        #region GetAllPersons

        [Fact]

        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> personResponsesList = _personsService.GetAllPersons();

            Assert.Empty(personResponsesList);
        }

        [Fact]

        public void GetAllPersons_ProperList()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            List<PersonAddRequest> personAddRequestsList = new List<PersonAddRequest>(){
               new PersonAddRequest() { Address = "a",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2001-07-05"),Email="a@a.com",Gender=GenderOptions.Male,PersonName="lol",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "b",CountryID = countryResponse2.CountryId,DateOfBirth = DateTime.Parse("2000-07-05"),Email="b@a.com",Gender=GenderOptions.Male,PersonName="hole",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "c",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2000-07-08"),Email="b@c.com",Gender=GenderOptions.Female,PersonName="ole",ReceiveNewsLetters=true}
           };

            List<PersonResponse> persons_response_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest temp in personAddRequestsList)
            {
                persons_response_from_add.Add(_personsService.AddPerson(temp));

            }

            List<PersonResponse> persons_response_from_get = _personsService.GetAllPersons();

            foreach (PersonResponse person_response in persons_response_from_add)
            {
                Assert.Contains(person_response, persons_response_from_get);
            }

        }
        #endregion


        #region GetFilteredPersons


        [Fact]

        public void GetFilteredPersons_EmptyString()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            List<PersonAddRequest> personAddRequestsList = new List<PersonAddRequest>(){
               new PersonAddRequest() { Address = "a",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2001-07-05"),Email="a@a.com",Gender=GenderOptions.Male,PersonName="lol",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "b",CountryID = countryResponse2.CountryId,DateOfBirth = DateTime.Parse("2000-07-05"),Email="b@a.com",Gender=GenderOptions.Male,PersonName="hole",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "c",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2000-07-08"),Email="b@c.com",Gender=GenderOptions.Female,PersonName="ole",ReceiveNewsLetters=true}
           };

            List<PersonResponse> persons_response_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest temp in personAddRequestsList)
            {
                persons_response_from_add.Add(_personsService.AddPerson(temp));

            }

            List<PersonResponse> persons_response_from_get = _personsService.GetFilteredPersons(nameof(PersonResponse.PersonName),"");

            foreach (PersonResponse person_response in persons_response_from_add)
            {
                Assert.Contains(person_response, persons_response_from_get);
            }

        }


        [Fact]

        public void GetFilteredPersons_ProperString()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            List<PersonAddRequest> personAddRequestsList = new List<PersonAddRequest>(){
               new PersonAddRequest() { Address = "a",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2001-07-05"),Email="a@a.com",Gender=GenderOptions.Male,PersonName="lol",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "b",CountryID = countryResponse2.CountryId,DateOfBirth = DateTime.Parse("2000-07-05"),Email="b@a.com",Gender=GenderOptions.Male,PersonName="hole",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "c",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2000-07-08"),Email="b@c.com",Gender=GenderOptions.Female,PersonName="ole",ReceiveNewsLetters=true}
           };

            List<PersonResponse> persons_response_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest temp in personAddRequestsList)
            {
                persons_response_from_add.Add(_personsService.AddPerson(temp));

            }

            List<PersonResponse> persons_response_from_get = _personsService.GetFilteredPersons(nameof(PersonResponse.PersonName), "ma");

            foreach (PersonResponse person_response in persons_response_from_add)
            {
                if(person_response.PersonName != null)
                {
                    if (person_response.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response, persons_response_from_get);
                    }
                }
            }

        }




        #endregion


        #region GetSortedOrder

        [Fact]

        public void GetSortedOrder_ByDESC()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Japan" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);

            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>(){
               new PersonAddRequest() { Address = "a",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2001-07-05"),Email="a@a.com",Gender=GenderOptions.Male,PersonName="lol",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "b",CountryID = countryResponse2.CountryId,DateOfBirth = DateTime.Parse("2000-07-05"),Email="b@a.com",Gender=GenderOptions.Male,PersonName="hole",ReceiveNewsLetters=true},
               new PersonAddRequest() { Address = "c",CountryID = countryResponse1.CountryId,DateOfBirth = DateTime.Parse("2000-07-08"),Email="b@c.com",Gender=GenderOptions.Female,PersonName="ole",ReceiveNewsLetters=true}
           };

            List<PersonResponse> persons_response_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest temp in personAddRequests)
            {
                persons_response_from_add.Add(_personsService.AddPerson(temp));

            }

            List<PersonResponse> allPersons = _personsService.GetAllPersons();

            List<PersonResponse> actualSortedList  = allPersons.OrderByDescending((temp) => temp.PersonName).ToList();

            List<PersonResponse> persons_response_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

           for(int i=0;i<allPersons.Count;i++)
            {
                Assert.Equal(actualSortedList[i], persons_response_from_sort[i]);
            }

        }

        #endregion

        #region UpdatePerson

        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _personsService.UpdatePerson(person_update_request);
            });
        }


        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = new PersonUpdateRequest() { PersonId = Guid.NewGuid() };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personsService.UpdatePerson(person_update_request);
            });
        }


        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryId, Email = "john@example.com", Address = "address...", Gender = GenderOptions.Male };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personsService.UpdatePerson(person_update_request);
            });

        }


        //First, add a new person and try to update the person name and email
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryId, Address = "Abc road", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = _personsService.GetPersonByPersonID(person_response_from_update.PersonId);

            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);

        }

        #endregion


        #region DeletePerson

        //If you supply an valid PersonID, it should return true
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_response_from_add = _countriesService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "Jones", Address = "address", CountryID = country_response_from_add.CountryId, DateOfBirth = Convert.ToDateTime("2010-01-01"), Email = "jones@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = _personsService.AddPerson(person_add_request);


            //Act
            bool isDeleted = _personsService.DeletePerson(person_response_from_add.PersonId);

            //Assert
            Assert.True(isDeleted);
        }


        //If you supply an invalid PersonID, it should return false
        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            Assert.False(isDeleted);
        }


        #endregion


    }
}

