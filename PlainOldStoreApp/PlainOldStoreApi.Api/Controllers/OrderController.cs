using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApi.Api.Dtos;
using PlainOldStoreApp.DataStorage;

namespace PlainOldStoreApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly Guid _ordersInvoiceID;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            _ordersInvoiceID = Guid.NewGuid();
        }

        // GET by id

        // GET by firstName lastName

        // POST all customer orders
        [HttpPost("order")]
        public async Task<ActionResult<Tuple<List<Order>, string>>> POSTAllOrders(AddOrder newOrder)
        {
            Guid customerId = newOrder.CustomerId;
            int storeId = newOrder.StoreLocation;

            List<Order> orders = new List<Order>();
            
            orders.Add(new(newOrder.ProductId, newOrder.ProductPrice, newOrder.ProductQuantiy));
            
            Tuple<List<Order>, string> postAllOrders = await _orderRepository.AddAllOrders(customerId, _ordersInvoiceID, storeId, orders);

            return postAllOrders;
        }
    }
}
