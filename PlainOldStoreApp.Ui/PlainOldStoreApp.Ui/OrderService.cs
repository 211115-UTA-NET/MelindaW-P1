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

        public async Task<List<Order>> GetAllOrdersByName(string firstName, string lastName)
        {
            Dictionary<string, string> orderQuery = new()
            {
                ["FirstName"] = firstName,
                ["LastName"] = lastName
            };

            string requestUri = QueryHelpers.AddQueryString("/api/order/firstName&lastName", orderQuery);

            HttpRequestMessage orderRequest = new(HttpMethod.Get, requestUri);

            orderRequest.Headers.Accept.Add(new(MediaTypeNames.Application.Json));

            HttpResponseMessage orderResponse;

            try
            {
                orderResponse = await _httpClient.SendAsync(orderRequest);
            }
            catch (ServerException ex)
            {
                throw new ServerException("network error", ex);
            }

            orderResponse.EnsureSuccessStatusCode();

            if (orderResponse.Content.Headers.ContentType?.MediaType != MediaTypeNames.Application.Json)
            {
                throw new ServerException();
            }

            List<Order>? orders = await orderResponse.Content.ReadFromJsonAsync<List<Order>>();

            if (orders == null)
            {
                throw new ServerException();
            }

            return orders;

            throw new NotImplementedException();
        }

        public async Task<Tuple<List<Order>, string>> PostAllOrders(List<Order> ordersMade)
        {
            OrderList orderList = new() { Orders = ordersMade };

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

            Tuple<List<Order>, string>? summery = await orderResponse.Content.ReadFromJsonAsync<Tuple<List<Order>, string>>();

            if (summery == null)
            {
                throw new ServerException();
            }
            
            return summery;
        }
    }
}
