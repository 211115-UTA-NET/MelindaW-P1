namespace PlainOldStoreApp.DataStorage
{
    public class Order
    {
        public Guid? CustomerId { get; }
        public int? StoreLocation { get; }
        internal decimal? OrderTotal { get; }
        public int? ProductId { get; }
        public decimal? ProductPrice { get; }
        public int? ProductQuantiy { get; }
        public string? ProductName { get; }
        public DateTime? DateTime { get; }

        private readonly IOrderRepository? _orderRepository;

        private readonly Guid _ordersInvoiceID = Guid.NewGuid();

        public Order(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        internal Order(string productName, int quantity, decimal productPrice)
        {
            ProductName = productName;
            ProductQuantiy = quantity;
            ProductPrice = productPrice;
        }
        public Order(int? productId, decimal? productPrice, int? quantity)
        {
            ProductId = productId;
            ProductPrice = productPrice;
            ProductQuantiy = quantity;
        }
        internal Order(string productName, decimal productPrice, int quantity, DateTime orderdate)
        {
            ProductName = productName;
            ProductPrice = productPrice;
            ProductQuantiy = quantity;
            DateTime = orderdate;
        }
        public Order(Guid cutomerId, int storeId, int productId, decimal productPrice, string productName, int productQuantiy)
        {
            CustomerId = cutomerId;
            StoreLocation = storeId;
            ProductId = productId;
            ProductPrice= productPrice;
            ProductName = productName;
            ProductQuantiy = productQuantiy;
        }
        public Tuple<List<Order>, string> PlaceCustomerOreder(Guid customerId, int storeId, List<Order> orders)
        {
            Tuple<List<Order>, string> getOrders = _orderRepository!.AddAllOrders(customerId, _ordersInvoiceID, storeId, orders).Result;
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