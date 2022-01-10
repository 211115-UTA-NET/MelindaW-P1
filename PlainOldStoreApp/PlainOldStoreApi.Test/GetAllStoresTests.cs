using Moq;
using PlainOldStoreApi.Api.Controllers;
using PlainOldStoreApp.DataStorage;
using System.Collections.Generic;
using Xunit;

namespace PlainOldStoreApi.Test
{
    public class GetAllStoresTests
    {
        [Fact]
        public void GetAllStoresTest()
        {
            //Arrange
            Dictionary<int, string> stores = new()
            {
                { 1, "Mountain View" },
                { 2, "San Jose" },
                { 3, "Edmond" }
            };
            var mock = new Mock<IStoreRepository>();
            mock.Setup(x => x.RetriveStores()).ReturnsAsync(stores);
            var controller = new StoreController(mock.Object);

            //Act
            var actual = controller.GetAllStores();
            //Assert
            Assert.Equal(stores.Keys, actual.Result.Value?.Keys);
            Assert.Equal(stores.Values, actual.Result.Value?.Values);
        }
    }
}