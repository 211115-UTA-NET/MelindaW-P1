using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui.Dtos
{
    public class Order
    {
        public Guid CustomerId { get; set; }
        public int StoreLocation { get; set; }
        public int? ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int? ProductQuantiy { get; set; }

        public Order() { }

        public Order(Guid customerId, int storeLocation, int? productId, decimal productPrice, int? productQuantity)
        {
            CustomerId = customerId;
            StoreLocation = storeLocation;
            ProductId = productId;
            ProductPrice = productPrice;
            ProductQuantiy = productQuantity;
        }
    }
}
