using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.DataStorage
{
    public class Store
    {
        private Dictionary<int, string> _stores = new Dictionary<int, string>();

        public string GetStore(int key)
        {
            string value = "";
            if (_stores.ContainsKey(key))
            {
                value = _stores[key];
            }
            return value;
        }

        private readonly IStoreRepository _storeRepository;

        public Store(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public Dictionary<int, string> GetStoresFromDatabase()
        {
            _stores = _storeRepository.RetriveStores().Result;
            return _stores;
        }
    }
}