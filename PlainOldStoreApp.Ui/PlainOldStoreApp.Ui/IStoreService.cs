using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public interface IStoreService
    {
        Task<Dictionary<int, string>> GetStoreListAsync();
    }
}
