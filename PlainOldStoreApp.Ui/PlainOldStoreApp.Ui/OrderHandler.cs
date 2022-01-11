using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    public class OrderHandler
    {
        private readonly IOrderService _orderService;

        public OrderHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<List<Order>> GetAllOrdersByCustomerName(string firstName, string lastName)
        {
            List<Order> orders = await _orderService.GetAllOrdersByName(firstName, lastName);
            return orders;
        }
    }
}
