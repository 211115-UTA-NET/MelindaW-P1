using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PlainOldStoreApp.DataStorage;
using System.ComponentModel.DataAnnotations;

namespace PlainOldStoreApi.Api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: Get by email
        [HttpGet]
        public async Task<ActionResult<bool>> GetCustomerEmailTrueOrFalse([FromQuery, Required] string email)
        {
            bool foundEmail = await _customerRepository.GetCustomerEmail(email);
            return foundEmail;
        }

        //GET: Get id by email
        [HttpGet("{email}")]
        public async Task<ActionResult<Guid>> GetCustomerIdByEmail(string email)
        {
            Guid customerId = await _customerRepository.SqlGetCustomerId(email);

            if (customerId == Guid.Empty)
            {
                return NotFound();
            }
            return customerId;
        }

        // GET: Get by firstName, lastName
        [HttpGet("{firstName},{lastName}")]
        public async Task<ActionResult<List<Customer>>> GetAll(string firstName, string lastName)
        {
            List<Customer> customers = await _customerRepository.GetAllCustomer(firstName, lastName);
            return customers;
        }

        // POST: Add new customer
        [HttpPost]
        public async Task<ActionResult<bool>> PostCustomer(
            string firstName,
            string lastName,
            string address,
            string city,
            string state,
            string zip,
            string email)
        {
            bool newCustomer;
            if (!await _customerRepository.GetCustomerEmail(email))
            {
                newCustomer = await _customerRepository.AddNewCustomer(firstName, lastName, address, city, state, zip, email);
                return newCustomer;
            }

            return BadRequest($"a customer with {email} already exists");
        }
    }
}
