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

    }
}
