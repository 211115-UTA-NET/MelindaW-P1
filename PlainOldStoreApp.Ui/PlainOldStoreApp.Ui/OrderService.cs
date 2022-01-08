using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    internal class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient = new();

        public OrderService(Uri serverUri)
        {
            _httpClient.BaseAddress = serverUri;
        }
        public Task<Tuple<List<Order>, string>> PostAllOrders(int customerId, int storeLocation, List<Order> ordersMade)
        {
            throw new NotImplementedException();
        }
    }
}
