using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.DataStorage
{
    public class Customer
    {
        internal Guid CustomerId { get; }
        public string? FirstName { get; }
        public string? LastName { get; }
        public string? Address1 { get; }
        public string? City { get; }
        public string? State { get; }
        public string? ZipCode { get; }
        public string? Email { get; }

        private readonly ICustomerRepository _customerRepository;

        public Customer(string email, ICustomerRepository customerRepository)
        {
            Email = email;
            _customerRepository = customerRepository;
        }

        public Customer(string firstName, string lastName, ICustomerRepository customerRepository)
        {
            FirstName = firstName;
            LastName = lastName;
            _customerRepository = customerRepository;
        }

        public Customer(
            string firstName,
            string lastName,
            string address1,
            string city,
            string state,
            string zipCode,
            string email,
            ICustomerRepository customerRepository)
        {
            FirstName = firstName;
            LastName = lastName;
            Address1 = address1;
            City = city;
            State = state;
            ZipCode = zipCode;
            Email = email;
            _customerRepository = customerRepository;
        }

        public Customer(
            Guid customerId,
            string firstName,
            string lastName,
            string address1,
            string city,
            string state,
            string zipCode,
            string email)
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

        public bool LookUpEmail()
        {
            bool foundEmail = _customerRepository.GetCustomerEmail(Email).Result;
            return foundEmail;
        }

        public List<Customer> LookUpName()
        {
            List<Customer> foundName = _customerRepository.GetAllCustomer(FirstName, LastName).Result;
            return foundName;
        }

        public bool AddCustomer()
        {
            bool isAdded = _customerRepository.AddNewCustomer(FirstName, LastName, Address1, City, State, ZipCode, Email).Result;
            return isAdded;
        }

        public Guid GetCustomerID()
        {
            Guid customerId = _customerRepository.SqlGetCustomerId(Email).Result;
            return customerId;
        }
    }
}
