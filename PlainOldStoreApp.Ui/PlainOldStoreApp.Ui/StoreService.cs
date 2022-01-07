using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    internal class StoreService : IStoreService
    {
        private readonly HttpClient _httpClient = new();

        public StoreService(Uri serverUri)
        {
            _httpClient.BaseAddress = serverUri;
        }
        public async Task<Dictionary<int, string>> GetStoreListAsync()
        {
            string requestUri = "/api/Store/stores";

            HttpRequestMessage storesRequest = new(HttpMethod.Get, requestUri);

            storesRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));

            HttpResponseMessage storesResponse;

            try
            {
                storesResponse = await _httpClient.SendAsync(storesRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            storesResponse.EnsureSuccessStatusCode();

            if (storesResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            Dictionary<int, string>? stores = await storesResponse.Content.ReadFromJsonAsync<Dictionary<int, string>>();

            if (stores == null)
            {
                throw new ServerException();
            }

            return stores;
        }
    }
}
