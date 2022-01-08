using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui.Dtos
{
    internal class Order
    {
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductQuantiy { get; set; }
    }
}
