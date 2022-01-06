using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApp.DataStorage;
using System.ComponentModel.DataAnnotations;

namespace PlainOldStoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET by store id
        [HttpGet("id")]
        public async Task<ActionResult<List<Product>>> GetAllStoreProductsAsync([FromQuery, Required]int id)
        {
            List<Product> products = await _productRepository.GetAllStoreProducts(id);
            return products;
        }
    }
}
