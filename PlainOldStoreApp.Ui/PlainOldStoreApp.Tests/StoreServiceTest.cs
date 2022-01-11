using Moq;
using PlainOldStoreApp.Ui;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PlainOldStoreApp.Tests
{
    public class StoreServiceTest
    {
        [Fact]
        public async Task GetAllStoresTest()
        {
            //Arrange
            Dictionary<int, string> stores = new()
            {
                { 1, "Mountain View" },
                { 2, "San Jose" },
                { 3, "Edmond" }
            };
            var mock = new Mock<IStoreService>();
            mock.Setup(x => x.GetStoreListAsync()).ReturnsAsync(stores);
            var storeList = new StoreList(mock.Object);
            //Act
            var actual = await storeList.GetAllStores();

            //Assert
            Assert.Equal(stores.Keys, actual.Keys);
            Assert.Equal(stores.Values.ToString(), actual.Values.ToString());
        }
    }
}
