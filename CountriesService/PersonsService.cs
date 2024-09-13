﻿using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using System;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;


namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countries;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countries = new CountriesService();
        }

        private PersonResponse PersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();

            personResponse.Country = _countries.GetCountryByCountryId(personResponse.CountryID)?.CountryName;

            return personResponse;




        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

            //Model Validation
            ValidationHelper.ModelValidation(personAddRequest);
           

            //Convert to Person
            Person person = personAddRequest.ToPerson();


            person.PersonId = Guid.NewGuid();

           

            //Add to the list
            _persons.Add(person);

            return PersonToPersonResponse(person);
            
        }

        public List<PersonResponse> GetAllPersons()
        {
           return _persons.Select((temp) => temp.ToPersonResponse()).ToList();
            
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if(personID == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault((temp) => temp.PersonId == personID);

            if(person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = this.GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
            {
                return matchingPersons;
            }

            switch(searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where((temp) =>(!string.IsNullOrEmpty(temp.PersonName))?temp.PersonName.Contains(searchBy,StringComparison.OrdinalIgnoreCase):true).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where((temp) => (!string.IsNullOrEmpty(temp.Email)) ? temp.Email.Contains(searchBy, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where((temp) => (temp.Gender!=null) ? temp.Gender.ToString().Contains(searchBy, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where((temp) => (temp.DateOfBirth != null) ? temp.DateOfBirth.Value.ToString("dd mmmm YYYY").Contains(searchBy, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;


                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where((temp) => (!string.IsNullOrEmpty(temp.Country)) ? temp.Country.Contains(searchBy, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where((temp) => (!string.IsNullOrEmpty(temp.Address)) ? temp.Address.Contains(searchBy, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;


                default: matchingPersons = allPersons;
                    break;


            }


            return matchingPersons;


        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.PersonId == personUpdateRequest.PersonId);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }


        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personID);
            if (person == null)
                return false;

            _persons.RemoveAll(temp => temp.PersonId == personID);

            return true;
        }
    }
}
