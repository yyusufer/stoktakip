// Repositories/ProductRepository.cs
using MySql.Data.MySqlClient;
using Takip.Models;
using Takip.Services;
namespace Takip.Repositories
{
    public class ProductRepository
    {
        private readonly DatabaseService _dbService;

        public ProductRepository(DatabaseService dbService)
        {
            _dbService = dbService;
        }

        // Tüm ürünleri getir
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using var conn = _dbService.GetConnection();
            conn.Open();

            using var cmd = new MySqlCommand("SELECT * FROM products", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Stock = reader.GetInt32("stock"),
                    Price = reader.GetDecimal("price")
                });
            }
            return products;
        }

        // Ürün ekle
        public void AddProduct(Product product)
        {
            if (product.Stock <= 0)
                throw new Exception("Stok 0'dan küçük olamaz!");
            if (product.Price <= 0)
                throw new Exception("Fiyat 0'dan küçük olamaz!");

            using var conn = _dbService.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand(
                "INSERT INTO products (name, stock, price) VALUES (@name, @stock, @price)", 
                conn);
            
            cmd.Parameters.AddWithValue("@name", product.Name);
            cmd.Parameters.AddWithValue("@stock", product.Stock);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.ExecuteNonQuery();
        }

        // Ürün sil
        public void DeleteProduct(int id)
        {
            using var conn = _dbService.GetConnection();
            conn.Open();
            using var cmd = new MySqlCommand("DELETE FROM products WHERE id = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }
    }
}