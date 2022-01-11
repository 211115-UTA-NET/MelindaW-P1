using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public class StoreList
    {
        private readonly IStoreService _storeService;

        public StoreList(IStoreService storeService)
        {
            _storeService = storeService;
        }
        public async Task<Dictionary<int, string>> GetAllStores()
        {
            Dictionary<int, string> stores;
            try
            {
                stores = await _storeService.GetStoreListAsync();
            }
            catch (ServerException)
            {
                Console.WriteLine("Unable to connect to server");
                return new Dictionary<int, string> { [-1] = "-1" };
            }
            return stores;
        }
    }
}
