namespace PlainOldStoreApp.DataStorage
{
    public class Product
    {
        public int? ProductId { get; }
        public string? ProductName { get; }
        public string? ProductDescription { get; }
        public decimal ProductPrice { get; }
        public int? ProductQuantiy { get; set; }
        public int StoreID { get; }

        private readonly IProductRepository _productRepository;

        public Product(int storeLocation, IProductRepository productRepository)
        {
            StoreID = storeLocation;
            _productRepository = productRepository;
        }
        public Product(int productId, string productName, string productDescription, decimal productPrice, int productQuantiy, int storeID)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductQuantiy = productQuantiy;
            StoreID = storeID;
        }

        public List<Product> GetStoreInventory()
        {
            List<Product> products = _productRepository.GetAllStoreProducts(StoreID).Result;
            return products;
        }
    }
}