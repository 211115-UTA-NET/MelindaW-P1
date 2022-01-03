namespace PlainOldStoreApp.DataStorage
{
    public interface IStoreRepository
    {
        Task<Dictionary<int, string>> RetriveStores();
    }
}
