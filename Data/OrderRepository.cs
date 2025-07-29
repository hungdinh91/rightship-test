using LegacyOrderService.Models;
using Microsoft.Data.Sqlite;

namespace LegacyOrderService.Data
{
    public class OrderRepository
    {
        private string _connectionString = $"Data Source={Path.Combine(AppContext.BaseDirectory, "orders.db")}";


        public void Save(Order order)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $@"
                    INSERT INTO Orders (CustomerName, ProductName, Quantity, Price)
                    VALUES ('@CustomerName', '@ProductName', @Quantity, @Price)";

                command.Parameters.AddWithValue("@CustomerName", order.CustomerName);
                command.Parameters.AddWithValue("@ProductName", order.ProductName);
                command.Parameters.AddWithValue("@Quantity", order.Quantity);
                command.Parameters.AddWithValue("@Price", order.Price);

                command.ExecuteNonQuery();
            }
        }

        public void SeedBadData()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Orders (CustomerName, ProductName, Quantity, Price) VALUES ('John', 'Widget', 9999, 9.99)";
            cmd.ExecuteNonQuery();
        }
    }
}
