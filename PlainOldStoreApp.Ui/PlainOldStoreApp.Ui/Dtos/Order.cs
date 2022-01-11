namespace PlainOldStoreApp.Ui.Dtos
{
    public class Order
    {
        public Guid? CustomerId { get; set; }
        public int? StoreLocation { get; set; }
        public int? ProductId { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductName { get; set; }
        public int? ProductQuantiy { get; set; }
        public DateTime? DateTime { get; set; }
        public Order() { }

        public Order(string productName, int productQuantity, decimal productPrice)
        {
            ProductName = productName;
            ProductQuantiy = productQuantity;
            ProductPrice = productPrice;
        }
        public Order(Guid customerId, int storeLocation, int? productId, decimal productPrice, string productName, int? productQuantity)
        {
            CustomerId = customerId;
            StoreLocation = storeLocation;
            ProductId = productId;
            ProductPrice = productPrice;
            ProductName = productName;
            ProductQuantiy = productQuantity;
        }
    }
}
