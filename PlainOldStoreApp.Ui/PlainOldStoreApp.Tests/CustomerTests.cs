using Moq;
using PlainOldStoreApp.Ui;
using PlainOldStoreApp.Ui.Dots;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PlainOldStoreApp.Tests
{
    public class CustomerTests
    {
        [Fact]
        public async Task IsCoustomerInDataBaseByEmail_Ture()
        {
            //Arrange
            string email = "m@g.com";
            bool isCoustomer = true;
            var mock = new Mock<ICustomerService>();
            mock.Setup(x => x.GetIfEmailFound(email)).ReturnsAsync(true);
            var customerHandler = new CustomerHandler(mock.Object);

            //Act
            var actual = await customerHandler.IsEmailFound(email);
            //Assert
            Assert.Equal(isCoustomer.ToString(), actual.ToString());
        }

        [Fact]
        public async Task IsCoustomerInDataBaseByEmail_False()
        {
            //Arrange
            string eamil = null;
            bool isCoustomer = false;
            var mock = new Mock<ICustomerService>();
            mock.Setup(x => x.GetIfEmailFound(eamil)).ReturnsAsync(false);
            var customerHandler = new CustomerHandler(mock.Object);

            //Act
            var actual = await customerHandler.IsEmailFound(eamil);
            //Assert
            Assert.Equal(isCoustomer.ToString(), actual.ToString());
        }

        [Fact]
        public async Task GetCustomerByNameTest()
        {
            //Arrange
            string firstName = "Melinda";
            string lastName = "Waggoner";
            List<Customer> customers = new List<Customer>();
            customers.Add(new("Melinda", "Waggoner", "1 Hacker Way", "Menlo Park", "CA", "94025", "m@g.com"));
            var mock = new Mock<ICustomerService>();
            mock.Setup(x => x.GetAllCustomersByFullName(firstName, lastName)).ReturnsAsync(customers);
            var customerHandler = new CustomerHandler(mock.Object);

            //Act
            var actual = await customerHandler.GetCustomerByName(firstName, lastName);
            //Assert
            Assert.Equal(customers, actual);
        }

        [Fact]
        public async Task GetCustomerIdByEmailTest()
        {
            //Arrange
            string email = "m@g.com";
            Guid customerId = Guid.NewGuid();
            var mock = new Mock<ICustomerService>();
            mock.Setup(x => x.GetCustomerId(email)).ReturnsAsync(customerId);
            var customerHandler = new CustomerHandler(mock.Object);

            //Act
            var actual = await customerHandler.GetCustomerIdByEmail(email);
            //Assert
            Assert.Equal(customerId, actual);
        }
    }
}
