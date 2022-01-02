namespace PlainOldStoreApp.DataStorage
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllStoreProducts(int storeLocation);
    }
}