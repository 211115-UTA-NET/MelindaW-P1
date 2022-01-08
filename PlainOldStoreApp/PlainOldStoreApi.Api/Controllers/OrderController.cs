using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApp.DataStorage;

namespace PlainOldStoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET by id

        // GET by firstName lastName

        // POST all customer orders
        [HttpPost("order")]
        public async Task<ActionResult<Tuple<List<Order>, string>>> POSTAllOrders(Guid CustomerId, Guid ordersInvoiceID, int storeId, List<Order> orders)
        {
            Tuple<List<Order>, string> postAllOrders = await _orderRepository.AddAllOrders(CustomerId, ordersInvoiceID, storeId, orders);

            return postAllOrders;
        }
    }
}
