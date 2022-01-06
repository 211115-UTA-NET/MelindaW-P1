using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui.Dots
{
    public class Customer
    {
        public Guid CustomerId { get; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? ZipCode { get; set; }
        public string? Email { get; set; }

        public Customer() { }
        public Customer(string firstName, string lastName, string address1, string city, string state, string zipCode, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Address1 = address1;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
        }
        public Customer(Guid customerId, string firstName, string lastName, string address1, string city, string state, string zipCode, string email)
        {
            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Address1 = address1;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
        }
    }
}
