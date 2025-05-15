using MySql.Data.MySqlClient;

namespace Takip.Config
{
    public class DatabaseConfig
    {
        private readonly string _connectionString;

        public DatabaseConfig()
        {
            _connectionString = "server=localhost;port=3306;database=stoktakipdb;user=stokuser;password=123456;";
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
