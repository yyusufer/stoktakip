using MySql.Data.MySqlClient;
using Takip.Models;
using Takip.Services;

namespace Takip.Repositories
{
    public class SaleRepository
    {
        private readonly DatabaseService _dbService;

        public SaleRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        public void AddSale(Sale sale)
        {
            using var conn = _dbService.GetConnection();
            conn.Open();
            if (sale.Quantity <= 0)
            {
                throw new ArgumentException("Satış miktarı 0'dan küçük olamaz.");
            }
            // Stok kontrolü
            using var checkCmd = new MySqlCommand("SELECT stock FROM products WHERE id = @productId", conn);
            checkCmd.Parameters.AddWithValue("@productId", sale.ProductId);
            var stock = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (stock < sale.Quantity)
                throw new Exception("Yeterli stok yok.");

            // Stok düşür
            using var updateCmd = new MySqlCommand("UPDATE products SET stock = stock - @quantity WHERE id = @productId", conn);
            updateCmd.Parameters.AddWithValue("@quantity", sale.Quantity);
            updateCmd.Parameters.AddWithValue("@productId", sale.ProductId);
            updateCmd.ExecuteNonQuery();

            // Satışı kaydet
            using var insertCmd = new MySqlCommand("INSERT INTO sales (product_id, personnel_id, quantity, price, time) VALUES (@productId, @personnelId, @quantity, @price, @time)", conn);
            insertCmd.Parameters.AddWithValue("@productId", sale.ProductId);
            insertCmd.Parameters.AddWithValue("@personnelId", sale.PersonnelId);
            insertCmd.Parameters.AddWithValue("@quantity", sale.Quantity);
            insertCmd.Parameters.AddWithValue("@price", sale.Price);
            insertCmd.Parameters.AddWithValue("@time", sale.Time);
            insertCmd.ExecuteNonQuery();
        }

        public List<Sale> GetAllSales()
        {
            var list = new List<Sale>();

            using var conn = _dbService.GetConnection();
            conn.Open();

            string query = @"SELECT  s.product_id, s.personnel_id, s.quantity, s.price, s.time
                             FROM sales s";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Sale
                {

                    ProductId = reader.GetInt32("product_id"),
                    PersonnelId = reader.GetInt32("personnel_id"),
                    Quantity = reader.GetInt32("quantity"),
                    Price = reader.GetDecimal("price"),
                    Time = reader.GetDateTime("time")
                });
            }

            return list;
        }
    }
}
