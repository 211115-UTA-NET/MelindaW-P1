using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public interface IProductService
    {
        Task<List<Product>> GetAllStoreProductsById(int id);
    }
}
