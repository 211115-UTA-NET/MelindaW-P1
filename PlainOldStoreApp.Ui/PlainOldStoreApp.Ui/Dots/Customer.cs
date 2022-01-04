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

        public Customer(string firstname, string lastName, string Address1, string City, string state, string zipCode, string Email)
        {
            FirstName = firstname;
            LastName = lastName;
            Address1 = Address1;
            City = City;
            State = State;
            ZipCode = ZipCode;
            Email = Email;
        }
    }
}
