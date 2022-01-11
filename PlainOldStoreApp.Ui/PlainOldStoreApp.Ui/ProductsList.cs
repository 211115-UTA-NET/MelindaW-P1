using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public class ProductsList
    {
        private readonly IProductService _productService;

        public ProductsList(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<Product>> GetAllStoreProducts(int storeLocation)
        {
            List<Product> allStoreProducts = await _productService.GetAllStoreProductsById(storeLocation);
            return allStoreProducts;
        }
    }
}
