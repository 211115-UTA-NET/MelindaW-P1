using Moq;
using PlainOldStoreApp.Ui;
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
    }
}
