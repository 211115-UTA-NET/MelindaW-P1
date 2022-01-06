using Microsoft.AspNetCore.WebUtilities;
using PlainOldStoreApp.Ui.Dots;
using System.Net.Http.Json;
using System.Net.Mime;

namespace PlainOldStoreApp.Ui
{
    internal class PlainOldStoreService : IPlainOldStoreService
    {
        private readonly HttpClient _httpClient = new();

        public PlainOldStoreService(Uri serverUri)
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
            Dictionary<string, string> query = new()
            {
                ["firstName"] = firstName,
                ["lastName"] = lastName
            };
            string requestUri = QueryHelpers.AddQueryString("/api/customer/firstName&lastName", query);

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
    }
}
