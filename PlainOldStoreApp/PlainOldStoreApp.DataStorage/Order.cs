namespace PlainOldStoreApp.DataStorage
{
    public class Order
    {
        internal Guid? CustomerId { get; }
        internal int? StoreId { get; }
        internal decimal? OrderTotal { get; }
        internal int? OrdersInvoiceID { get; }
        internal int? ProductId { get; }
        public decimal? ProductPrice { get; }
        public int? Quantity { get; }
        public string? ProductName { get; }
        public DateTime? DateTime { get; }

        private readonly IOrderRepository? _orderRepository;

        public Order(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        internal Order(string productName, int quantity, decimal productPrice)
        {
            ProductName = productName;
            Quantity = quantity;
            ProductPrice = productPrice;
        }
        public Order(int? productId, decimal productPrice, int? quantity)
        {
            ProductId = productId;
            ProductPrice = productPrice;
            Quantity = quantity;
        }
        internal Order(string productName, decimal productPrice, int quantity, DateTime orderdate)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            Quantity = quantity;
            DateTime = orderdate;
        }

        public Tuple<List<Order>, string> PlaceCustomerOreder(Guid customerId, int storeId, List<Order> orders)
        {
            Tuple<List<Order>, string> getOrders = _orderRepository!.AddAllOrders(customerId, storeId, orders).Result;
            return getOrders;
        }

        public List<Order> GetAllCustomerOrders(string firstname, string lastName)
        {
            List<Order> orders = _orderRepository!.GetAllCoustomerOrders(firstname, lastName).Result;
            return orders;
        }

        public List<Order> GetAllStoreOrders(int storeID)
        {
            List<Order> orders = _orderRepository!.GetAllStoreOrders(storeID).Result;
            return orders;
        }
    }
}