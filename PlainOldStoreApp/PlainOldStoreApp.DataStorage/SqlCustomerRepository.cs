using Microsoft.Data.SqlClient;

namespace PlainOldStoreApp.DataStorage
{
    public class SqlCustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public SqlCustomerRepository(string connetionString)
        {
            _connectionString = connetionString;
        }
        /// <summary>
        /// Queries the PosaDatabase for an customer's email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A bool value that is true if the email is found</returns>
        public async Task<bool> GetCustomerEmail(string? email)
        {
            string emailSQL = "";

            using SqlConnection connection = new(_connectionString);

            using SqlCommand sqlCommand = new(
                @"SELECT Email
                    FROM Posa.Customer
                    WHERE Email=@email",
                connection);

            sqlCommand.Parameters.AddWithValue("@email", email);
            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = sqlCommand.ExecuteReader();

                if (await reader.ReadAsync())
                {
                    emailSQL = reader.GetString(0);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
            if (string.Equals(emailSQL, email, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Queries the Posa database for a customer base on their first and last name
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>List of customers</returns>
        public async Task<List<Customer>> GetAllCustomer(string? firstName, string? lastName)
        {
            List<Customer> customers = new();
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand sqlCommand = new(
                @"SELECT * FROM Posa.Customer
                WHERE FirstName = @firstName
                AND LastName = @lastName", connection);

            sqlCommand.Parameters.AddWithValue("@firstName", firstName);
            sqlCommand.Parameters.AddWithValue("@lastName", lastName);

            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = sqlCommand.ExecuteReader();

                while (await reader.ReadAsync())
                {
                    customers.Add(new(
                        reader.GetGuid(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7)));
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }
            return customers;
        }

        /// <summary>
        /// Add a customer to the Posa database
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="address1"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zip"></param>
        /// <param name="email"></param>
        /// <returns>True if the customer is added</returns>
        public async Task<bool> AddNewCustomer(
            string? firstName,
            string? lastName,
            string? address1,
            string? city,
            string? state,
            string? zip,
            string email
            )
        {
            using SqlConnection connection = new SqlConnection(_connectionString);

            string sqlString = @"INSERT INTO Posa.Customer
                (
                    FirstName,
                    LastName,
                    Address1,
                    City,
                    State,
                    Zip,
                    Email
                )
                VALUES
                (
                    @firstName,
                    @lastName, 
                    @address1,
                    @city,
                    @state,
                    @zip,
                    @email);";

            int isAdded = 0;
            try
            {
                await connection.OpenAsync();
                using SqlCommand sqlCommand = new(sqlString, connection);

                sqlCommand.Parameters.AddWithValue("@firstName", firstName);
                sqlCommand.Parameters.AddWithValue("@lastName", lastName);
                sqlCommand.Parameters.AddWithValue("@address1", address1);
                sqlCommand.Parameters.AddWithValue("@city", city);
                sqlCommand.Parameters.AddWithValue("@state", state);
                sqlCommand.Parameters.AddWithValue("@zip", zip);
                sqlCommand.Parameters.AddWithValue("@email", email);

                isAdded = sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }

            bool isCustomerAdded = false;
            if (isAdded > 0)
            {
                isCustomerAdded = true;
            }
            return isCustomerAdded;
        }

        /// <summary>
        /// Queries the Posa database for a customer's id base on their email.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>customerId</returns>
        public async Task<Guid> SqlGetCustomerId(string email)
        {
            Guid customerId = new();
            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand sqlCommand = new(
                @"SELECT CustomerID
                FROM Posa.Customer
                WHERE Email=@email;", connection);

            sqlCommand.Parameters.AddWithValue("@email", email);

            try
            {
                await connection.OpenAsync();
                using SqlDataReader reader = sqlCommand.ExecuteReader();
                if (await reader.ReadAsync())
                {
                    customerId = reader.GetGuid(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                await connection.CloseAsync();
            }

            return customerId;
        }
    }
}
