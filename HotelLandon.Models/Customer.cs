using Microsoft.AspNetCore.Mvc;

namespace HotelLandon.Models
{
    //[BindProperties]
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public Customer()
        //{

        //}

        //public Customer(string firstName, string lastName)
        //{
        //    FirstName = firstName;
        //    LastName = lastName;
        //}
    }
}
