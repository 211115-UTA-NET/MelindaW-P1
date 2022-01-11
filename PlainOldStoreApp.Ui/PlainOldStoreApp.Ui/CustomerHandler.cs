using PlainOldStoreApp.Ui.Dots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public class CustomerHandler
    {
        private readonly ICustomerService _customerService;

        public CustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<bool> IsEmailFound(string email)
        {
            bool foundEmail = await _customerService.GetIfEmailFound(email);

            return foundEmail;
        }

        public async Task<List<Customer>> GetCustomerByName(string firstName, string lastName)
        {
            List<Customer> foundCustomers = await _customerService.GetAllCustomersByFullName(firstName, lastName);
            return foundCustomers;
        }

        public async Task<Guid> GetCustomerIdByEmail(string email)
        {
            Guid customerId = await _customerService.GetCustomerId(email);
            return customerId;
        }

    }
}
