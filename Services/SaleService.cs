using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Takip.Models;

namespace Takip.Services
{
    public class SaleService
    {
        private readonly DatabaseService _db;

        public SaleService(DatabaseService db)
        {
            _db = db;
        }

        public List<SaleWithDetails> GetAllSales()
        {
            var sales = new List<SaleWithDetails>();

            using var conn = _db.GetConnection();
            conn.Open();

            string query = @"
                SELECT s.Id, p.Name AS ProductName, per.Name AS PersonnelName, s.Time, s.Price
                FROM Sale s
                JOIN Product p ON s.ProductId = p.Id
                JOIN Personnel per ON s.PersonnelId = per.Id";

            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                sales.Add(new SaleWithDetails
                {
                    Id = reader.GetInt32("Id"),
                    ProductName = reader.GetString("ProductName"),
                    PersonnelName = reader.GetString("PersonnelName"),
                    Time = reader.GetDateTime("Time"),
                    Price = reader.GetDecimal("Price")
                });
            }

            return sales;
        }
    }
}
