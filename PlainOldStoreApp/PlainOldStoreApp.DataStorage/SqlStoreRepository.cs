using Microsoft.Data.SqlClient;

namespace PlainOldStoreApp.DataStorage
{
    public class SqlStoreRepository : IStoreRepository
    {
        private readonly string _connectionString;

        public SqlStoreRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Queries the PosaDatabase and returns a list of stores
        /// </summary>
        /// <returns>Dictionary<int, string></int></returns>
        public async Task<Dictionary<int, string>> RetriveStores()
        {
            Dictionary<int, string> stores = new Dictionary<int, string>();
            using SqlConnection connection = new SqlConnection(_connectionString);


            using SqlCommand sqlCommand = new(@"SELECT * FROM Posa.Stores", connection);

            try
            {
                await connection.OpenAsync();
                using SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (await sqlDataReader.ReadAsync())
                {
                    stores.Add(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1));
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
            return stores;
        }
    }
}