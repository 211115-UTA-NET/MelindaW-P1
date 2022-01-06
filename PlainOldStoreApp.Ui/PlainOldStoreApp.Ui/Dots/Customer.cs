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
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? Address1 { get; }
        public string? City { get; }
        public string? State { get; }
        public string? ZipCode { get; }
        public string? Email { get; }

        public Customer(Guid customerId, string firstname, string lastName, string address1, string city, string state, string zipCode, string email)
        {
            CustomerId = customerId;
            FirstName = firstname;
            LastName = lastName;
            Address1 = address1;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
        }
    }
}
