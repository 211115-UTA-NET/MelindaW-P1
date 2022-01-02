using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.DataStorage
{
    public interface IStoreRepository
    {
        Task<Dictionary<int, string>> RetriveStores();
    }
}
