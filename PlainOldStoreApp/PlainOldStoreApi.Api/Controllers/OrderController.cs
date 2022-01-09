using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApi.Api.Dtos;
using PlainOldStoreApp.DataStorage;
using System.ComponentModel.DataAnnotations;

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

        // GET by Store id
        [HttpGet("storeId")]
        public async Task<ActionResult<List<Order>>> GetStoreOrdersByStoreId([FromQuery, Required]int storeId)
        {
            List<Order> storeOrders = await _orderRepository.GetAllStoreOrders(storeId);

            return storeOrders;
        }

        // GET by firstName lastName
        [HttpGet("firstName&lastName")]
        public async Task<ActionResult<List<Order>>> GetCustomerOrder([FromQuery, Required] string firstName, [FromQuery, Required] string lastName)
        {
            List<Order> customerOrders = await _orderRepository.GetAllCoustomerOrders(firstName, lastName);

            return customerOrders;
        }

        // POST all customer orders
        [HttpPost("order")]
        public async Task<ActionResult<Tuple<List<Order>, string>>> POSTAllOrders(OrderList newOrder)
        {
            var orders = newOrder.Orders;
            Guid customerId = Guid.Empty;
            int storeId = 0;
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
