namespace PlainOldStoreApp.Ui.Dtos
{
    public class Product
    {
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int? ProductQuantiy { get; set; }
        public int StoreID { get; set; }

        public Product() { }
        public Product(int productId, string productName, string? productDescription, decimal productPrice, int productQuantiy, int storeID)
        {
            ProductId = productId;
            ProductName = productName;
            ProductDescription = productDescription;
            ProductPrice = productPrice;
            ProductQuantiy = productQuantiy;
            StoreID = storeID;
        }
    }
}
