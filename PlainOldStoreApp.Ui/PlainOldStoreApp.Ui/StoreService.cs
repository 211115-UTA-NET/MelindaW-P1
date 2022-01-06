using System;
using System.Collections.Generic;
using System.Linq;
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
        public Task<Dictionary<int, string>> GetStoreListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
