using Microsoft.Data.SqlClient;
using System.Text;

namespace PlainOldStoreApp.DataStorage
{
    public class SqlOrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public SqlOrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="storeId"></param>
        /// <param name="orders"></param>
        /// <returns></returns>
        public async Task<Tuple<List<Order>, string>> AddAllOrders(Guid? customerId, Guid ordersInvoiceID, int? storeId, List<Order> orders)
        {
            decimal? orderTotal = 0;
            foreach (Order order in orders)
            {
                orderTotal += order.ProductPrice * order.ProductQuantiy;
            }

            using SqlConnection sqlConnection = new(_connectionString);

            string sqlUpdateString =
                @"UPDATE Posa.Inventory
                SET Quantity = (Quantity - @quantity)
                WHERE StoreID=@storeID
                AND ProductID=@ProductId;";

            await sqlConnection.OpenAsync();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            try
            {
                foreach (Order order in orders)
                {
                    using SqlCommand sqlUpdateCommand = new(sqlUpdateString, sqlConnection, sqlTransaction);

                    sqlUpdateCommand.Parameters.AddWithValue("@quantity", order.ProductQuantiy);
                    sqlUpdateCommand.Parameters.AddWithValue("@storeID", storeId);
                    sqlUpdateCommand.Parameters.AddWithValue("@ProductId", order.ProductId);

                    sqlUpdateCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                try
                { 
                    sqlTransaction.Rollback();
                    return new Tuple<List<Order>, string>(orders, "Could not complete order at this time.");
                }
                catch
                {
                    throw new Exception(ex.Message);
                }
            }
            sqlTransaction.Commit();
            await sqlConnection.CloseAsync();

            await sqlConnection.OpenAsync();

            using SqlCommand sqlCommand = new(
                @"INSERT INTO Posa.OrdersInvoice
                (
                    OrdersInvoiceID,
                    CustomerID,
                    StoreID,
                    OrderTotal
                )
                VALUES
                (
                    @ordersInvoiceID,
                    @customerId,
                    @storeID,
                    @orderTotal);", sqlConnection);

            sqlCommand.Parameters.AddWithValue("@ordersInvoiceID", ordersInvoiceID);
            sqlCommand.Parameters.AddWithValue("@customerId", customerId);
            sqlCommand.Parameters.AddWithValue("@storeID", storeId);
            sqlCommand.Parameters.AddWithValue("@orderTotal", orderTotal);

            sqlCommand.ExecuteNonQuery();

            await sqlConnection.CloseAsync();

            string sqlString =
                @"INSERT INTO Posa.CustomerOrders
                (
                    OrdersInvoiceID,
                    ProductID,
                    ProductPrice,
                    Quantity
                )
                VALUES
                (
                    @ordersInvoiceID,
                    @productId,
                    @productPrice,
                    @quantity);";

            await sqlConnection.OpenAsync();

            foreach (Order order in orders)
            {
                using SqlCommand sqlCommandOrders = new(sqlString, sqlConnection);

                sqlCommandOrders.Parameters.AddWithValue("@ordersInvoiceID", ordersInvoiceID);
                sqlCommandOrders.Parameters.AddWithValue("@productId", order.ProductId);
                sqlCommandOrders.Parameters.AddWithValue("@productPrice", order.ProductPrice);
                sqlCommandOrders.Parameters.AddWithValue("@quantity", order.ProductQuantiy);

                sqlCommandOrders.ExecuteNonQuery();
            }

            await sqlConnection.CloseAsync();

            List<Order> orderItems = new List<Order>();

            string sqlGetOrderString =
                @"SELECT CustomerID, Posa.OrdersInvoice.StoreID, Posa.Products.ProductID, Posa.CustomerOrders.ProductPrice, ProductName, Quantity, Posa.OrdersInvoice.OrderTime
                    FROM Posa.CustomerOrders
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    INNER JOIN Posa.OrdersInvoice ON Posa.CustomerOrders.OrdersInvoiceID=Posa.OrdersInvoice.OrdersInvoiceID
                    WHERE OrdersInvoice.OrdersInvoiceID=@ordersInvoiceID;";

            await sqlConnection.OpenAsync();

            using SqlCommand sqlGetOrderCommand = new(sqlGetOrderString, sqlConnection);

            sqlGetOrderCommand.Parameters.AddWithValue("@ordersInvoiceID", ordersInvoiceID);

            using SqlDataReader readOrder = sqlGetOrderCommand.ExecuteReader();

            while (await readOrder.ReadAsync())
            {
                orderItems.Add(new(
                    readOrder.GetGuid(0),
                    readOrder.GetInt32(1),
                    readOrder.GetInt32(2),
                    readOrder.GetDecimal(3),
                    readOrder.GetString(4),
                    readOrder.GetInt32(5),
                    readOrder.GetDateTime(6)));
            }
            await sqlConnection.CloseAsync();

            string orderSummery = "";

            string sqlGetOrderSummeryString =
                @"SELECT StoreCity, OrderTime, OrderTotal
                    FROM Posa.OrdersInvoice
                    INNER JOIN Posa.Stores ON Posa.OrdersInvoice.StoreID=Posa.Stores.StoreID
                    WHERE OrdersInvoiceID=@ordersInvoiceID;";

            await sqlConnection.OpenAsync();

            using SqlCommand sqlGetOrderSummery = new(sqlGetOrderSummeryString, sqlConnection);

            sqlGetOrderSummery.Parameters.AddWithValue("@ordersInvoiceID", ordersInvoiceID);

            using SqlDataReader readSummery = sqlGetOrderSummery.ExecuteReader();

            if (await readSummery.ReadAsync())
            {
                orderSummery = $"{readSummery.GetString(0)}\t{readSummery.GetDateTime(1)}\t{readSummery.GetDecimal(2)}";
            }

            await sqlConnection.CloseAsync();
            
            return new Tuple<List<Order>, string>(orderItems, orderSummery);
        }

        public async Task<List<Order>> GetAllCoustomerOrders(string fisrtName, string lastName)
        {
            List<Order> allCustomerOrders = new List<Order>();

            string sqlGetAllCustomerOrdersString =
                @"SELECT FirstName, LastName, Posa.Customer.CustomerID, Posa.OrdersInvoice.StoreID, Posa.CustomerOrders.ProductID, Posa.CustomerOrders.ProductPrice, ProductName, Quantity, Posa.OrdersInvoice.OrderTime
                    FROM Posa.Customer
                    INNER JOIN Posa.OrdersInvoice ON Posa.Customer.CustomerID=Posa.OrdersInvoice.CustomerID
                    INNER JOIN Posa.CustomerOrders ON Posa.OrdersInvoice.OrdersInvoiceID=Posa.OrdersInvoice.OrdersInvoiceID
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    WHERE FirstName = @firstName
                    AND LastName = @lastName;";

            using SqlConnection sqlConnection = new(_connectionString);
            await sqlConnection.OpenAsync();
            using SqlCommand sqlGetAllCustomerOrders = new(sqlGetAllCustomerOrdersString, sqlConnection);
            sqlGetAllCustomerOrders.Parameters.AddWithValue("@firstName", fisrtName);
            sqlGetAllCustomerOrders.Parameters.AddWithValue("@lastName", lastName);
            using SqlDataReader dataReader = sqlGetAllCustomerOrders.ExecuteReader();

            while (await dataReader.ReadAsync())
            {
                allCustomerOrders.Add(new(
                    dataReader.GetGuid(2),
                    dataReader.GetInt32(3),
                    dataReader.GetInt32(4),
                    dataReader.GetDecimal(5),
                    dataReader.GetString(6),
                    dataReader.GetInt32(7),
                    dataReader.GetDateTime(8)));
            }
            await sqlConnection.CloseAsync();
            return allCustomerOrders;
        }

        public async Task<List<Order>> GetAllStoreOrders(int storeID)
        {
            List<Order> allStoreOrders = new List<Order>();

            string sqlGetAllStoreOrdersString =
                @"SELECT Posa.OrdersInvoice.StoreID, ProductName, Posa.CustomerOrders.ProductPrice, Quantity, Posa.OrdersInvoice.OrderTime, Posa.CustomerOrders.OrderLineID
                    FROM Posa.OrdersInvoice
                    INNER JOIN Posa.CustomerOrders ON Posa.OrdersInvoice.OrdersInvoiceID=Posa.OrdersInvoice.OrdersInvoiceID
                    INNER JOIN Posa.Products ON Posa.CustomerOrders.ProductID=Posa.Products.ProductID
                    WHERE Posa.OrdersInvoice.StoreID=@storeID;";

            using SqlConnection sqlConnection = new(_connectionString);
            await sqlConnection.OpenAsync();
            using SqlCommand sqlGetAllStoreOrders = new(sqlGetAllStoreOrdersString, sqlConnection);
            sqlGetAllStoreOrders.Parameters.AddWithValue("@storeID", storeID);
            using SqlDataReader dataReader = sqlGetAllStoreOrders.ExecuteReader();

            while (await dataReader.ReadAsync())
            {
                allStoreOrders.Add(new(
                    dataReader.GetString(1),
                    dataReader.GetDecimal(2),
                    dataReader.GetInt32(3),
                    dataReader.GetDateTime(4)));
            }
            await sqlConnection.CloseAsync();
            return allStoreOrders;
        }
    }
}