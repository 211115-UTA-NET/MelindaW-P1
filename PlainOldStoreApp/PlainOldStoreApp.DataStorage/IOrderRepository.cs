namespace PlainOldStoreApp.DataStorage
{
    public interface IOrderRepository
    {
        Task<Tuple<List<Order>, string>> AddAllOrders(Guid? customerId, Guid ordersInvoiceId, int? storeId, List<Order> orders);

        Task<List<Order>> GetAllStoreOrders(int storeID);

        Task<List<Order>> GetAllCoustomerOrders(string firstName, string lastName);
    }
}