using Microsoft.AspNetCore.WebUtilities;
using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
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
        public async Task<Tuple<List<Order>, string>> PostAllOrders(List<Order> ordersMade)
        {
            OrderList orderList = new OrderList();
            orderList = new() { Orders = ordersMade };

            HttpRequestMessage orderRequest = new(HttpMethod.Post, "/api/Order/order");

            orderRequest.Content = new StringContent(JsonSerializer.Serialize(orderList), Encoding.UTF8, MediaTypeNames.Application.Json);

            HttpResponseMessage orderResponse;
            try
            {
                orderResponse = await _httpClient.SendAsync(orderRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            if (orderResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            Tuple<List<Order>, string> summery = await orderResponse.Content.ReadFromJsonAsync<Tuple<List<Order>, string>>();

            if (summery == null)
            {
                throw new ServerException();
            }
            
            return summery;

            throw new NotImplementedException();
        }
    }
}
