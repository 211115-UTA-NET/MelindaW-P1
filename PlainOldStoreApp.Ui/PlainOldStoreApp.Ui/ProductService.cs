using Microsoft.AspNetCore.WebUtilities;
using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    internal class ProductService : IProductService
    {
        private readonly HttpClient _httpClient = new();
        public ProductService(Uri serverUri)
        {
            _httpClient.BaseAddress = serverUri;
        }

        public async Task<List<Product>> GetAllStoreProductsById(int storeID)
        {
            Dictionary<string, string> productQuery = new()
            {
                ["id"] = storeID.ToString()
            };
            string requestUri = QueryHelpers.AddQueryString("/api/product/id", productQuery);

            HttpRequestMessage productRequest = new(HttpMethod.Get, requestUri);

            productRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));

            HttpResponseMessage produtResponse;

            try
            {
                produtResponse = await _httpClient.SendAsync(productRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            produtResponse.EnsureSuccessStatusCode();

            if (produtResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            List<Product>? products = await produtResponse.Content.ReadFromJsonAsync<List<Product>>();

            if (products == null)
            {
                throw new ServerException();
            }
            return products;
        }
    }
}
