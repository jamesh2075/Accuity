using System;
using System.Collections.Generic;
using System.Text;

namespace AccuityClient
{
    public class Person
    {
        public Person(string id, string firstName, string lastName, string city, string state, string country)
        {
            this.id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.City = city;
            this.State = state;
            this.Country = country;
        }
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
    }
}
