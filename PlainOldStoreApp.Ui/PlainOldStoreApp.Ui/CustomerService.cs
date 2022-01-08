using Microsoft.AspNetCore.WebUtilities;
using PlainOldStoreApp.Ui.Dots;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace PlainOldStoreApp.Ui
{
    internal class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient = new();

        public CustomerService(Uri serverUri)
        {
            _httpClient.BaseAddress = serverUri;
        }

        public async Task<bool> GetIfEmailFound(string email)
        {
            Dictionary<string, string> emailQuery = new()
            {
                ["email"] = email
            };
            string requestUri = QueryHelpers.AddQueryString("/api/customer/email", emailQuery);

            HttpRequestMessage emailRequest = new(HttpMethod.Get, requestUri);

            emailRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));

            HttpResponseMessage emailResponse;

            try
            {
                emailResponse = await _httpClient.SendAsync(emailRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            emailResponse.EnsureSuccessStatusCode();

            if (emailResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            string isEmailFound = await emailResponse.Content.ReadAsStringAsync();

            if (isEmailFound == "false")
            {
                return false;
            }
            return true;
        }

        public async Task<List<Customer>> GetAllCustomersByFullName(string firstName, string lastName)
        {
            Dictionary<string, string> customersQuery = new()
            {
                ["firstName"] = firstName,
                ["lastName"] = lastName
            };
            string requestUri = QueryHelpers.AddQueryString("/api/customer/firstName&lastName", customersQuery);

            HttpRequestMessage customerRequest = new(HttpMethod.Get, requestUri);

            customerRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));

            HttpResponseMessage customerResponse;
            try
            {
                customerResponse = await _httpClient.SendAsync(customerRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            customerResponse.EnsureSuccessStatusCode();

            if (customerResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            List<Customer>? customers = await customerResponse.Content.ReadFromJsonAsync<List<Customer>>();

            if (customers == null)
            {
                throw new ServerException();
            }

            return customers;
        }

        public async Task<Guid> GetCustomerId(string email)
        {
            Dictionary<string, string> customerIdQuery = new()
            {
                ["email"] = email
            };
            string requestUri = QueryHelpers.AddQueryString("/api/customer/id", customerIdQuery);

            HttpRequestMessage customerIdRequest = new(HttpMethod.Get, requestUri);

            customerIdRequest.Headers.Accept.Add(new (MediaTypeNames.Application.Json));

            HttpResponseMessage customerIdResponse;
            try
            {
                customerIdResponse = await _httpClient.SendAsync(customerIdRequest);
            }
            catch(ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            customerIdResponse.EnsureSuccessStatusCode();

            if (customerIdResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            Guid customerId = await customerIdResponse.Content.ReadFromJsonAsync<Guid>();

            return customerId;
        }

        public async Task<bool> PostNewCustomer(Customer newCustomer)
        {
            Customer addCustomer = new()
            {
                FirstName = newCustomer.FirstName,
                LastName = newCustomer.LastName,
                Address1 = newCustomer.Address1,
                City = newCustomer.City,
                State = newCustomer.State,
                ZipCode = newCustomer.ZipCode,
                Email = newCustomer.Email
            };
            HttpRequestMessage newCustomerRequest = new(HttpMethod.Post, "/api/customer");

            newCustomerRequest.Content = new StringContent(JsonSerializer.Serialize(addCustomer), Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage newCustomerResponse;
            try
            {
                newCustomerResponse = await _httpClient.SendAsync(newCustomerRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            bool result = await newCustomerResponse.Content.ReadFromJsonAsync<bool>();

            return result;
        }
    }
}
