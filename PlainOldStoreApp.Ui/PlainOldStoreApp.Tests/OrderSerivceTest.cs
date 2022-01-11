using Moq;
using PlainOldStoreApp.Ui;
using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PlainOldStoreApp.Tests
{
    public class OrderSerivceTest
    {
        [Fact]
        public async Task OrderSerivceTestGetAllOrdersByCustomerName()
        {
            //Arrange
            string firstName = "John";
            string lastName = "Smith";
            List<Order> orders = new List<Order>();
            Guid customerId = Guid.NewGuid();
            orders.Add(new(customerId, 1, 1, 20, "Some ProductName", 3));
            var mock = new Mock<IOrderService>();
            mock.Setup(x => x.GetAllOrdersByName(firstName, lastName)).ReturnsAsync(orders);
            var orderHandler = new OrderHandler(mock.Object);
            //Act
            var actual = await orderHandler.GetAllOrdersByCustomerName(firstName, lastName);

            //Assert
            Assert.Equal(orders, actual);
        }
    }
}
