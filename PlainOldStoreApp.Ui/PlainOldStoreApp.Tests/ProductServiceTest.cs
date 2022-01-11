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
    public class ProductServiceTest
    {
        [Fact]
        public async Task GetAllStoreProductsTest()
        {
            //Arrange
            int storeID = 1;
            var products = new List<Product>();
            products.Add(new(1, "Dress", "Green Dress", 19.99M, 3, 1));
            var mock = new Mock<IProductService>();
            mock.Setup(x => x.GetAllStoreProductsById(storeID)).ReturnsAsync(products);
            var productsList = new ProductsList(mock.Object);

            //Act
            var actual = await productsList.GetAllStoreProducts(storeID);

            //Assert
            Assert.Equal(products, actual);
        }
    }
}
