using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui.Dtos
{
    internal class Order
    {
        public int? ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int? ProductQuantiy { get; set; }

        public Order() { }

        public Order(int? productId, decimal productPrice, int? productQuantity)
        {
            ProductId = productId;
            ProductPrice = productPrice;
            ProductQuantiy = productQuantity;
        }
    }
}
