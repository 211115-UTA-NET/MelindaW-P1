using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApp.DataStorage;

namespace PlainOldStoreApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;

        public StoreController(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        // GET all stores
        [HttpGet("stores")]
        public async Task<ActionResult<Dictionary<int, string>>> GetAllStores()
        {
            Dictionary<int, string> stores = await _storeRepository.RetriveStores();
            return stores;
        }
    }
}
