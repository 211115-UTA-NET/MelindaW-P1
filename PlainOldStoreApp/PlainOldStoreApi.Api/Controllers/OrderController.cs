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
        public async Task<ActionResult<Tuple<List<Order>, string>>> POSTAllOrders(OrderList newOrder)
        {
            var orders = newOrder.Orders;
            Guid customerId = Guid.Empty;
            int storeId =0;
            List<Order> ordersList = new List<Order>();
            foreach (var order in orders)
            {
                customerId = order.CustomerId;
                storeId = order.StoreLocation;
                ordersList.Add(new(order.ProductId, order.ProductPrice, order.ProductQuantiy));
            }
            
            
            
            Tuple<List<Order>, string> postAllOrders = await _orderRepository.AddAllOrders(customerId, _ordersInvoiceID, storeId, ordersList);

            return postAllOrders;
        }
    }
}
