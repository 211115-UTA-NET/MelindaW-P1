using PlainOldStoreApp.Ui.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainOldStoreApp.Ui
{
    internal interface IOrderService
    {
        Task<Tuple<List<Order>, string>> PostAllOrders(int customerId, int storeLocation, List<Order> ordersMade);
    }
}
