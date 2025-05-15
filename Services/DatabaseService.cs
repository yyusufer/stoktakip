using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text.Json;
using Takip.Models;

namespace Takip.Services
{
    public class DatabaseService
    {
        private readonly string configPath = Path.Combine(Directory.GetCurrentDirectory(), "connection.json");
        private readonly DatabaseConfigViewModel _config;

        public DatabaseService()
        {
            if (!File.Exists(configPath))
                throw new FileNotFoundException("Veritabanı yapılandırma dosyası bulunamadı: " + configPath);

            var json = File.ReadAllText(configPath);
            _config = JsonSerializer.Deserialize<DatabaseConfigViewModel>(json) ?? throw new Exception("Yapılandırma dosyası bozuk. Tekrar deneyin");
            
        }

        public MySqlConnection GetConnection()
        {
            string? host = _config.HostType == "ip" ? _config.HostIp : "localhost";
            string connStr = $"Server={host};Port={_config.Port};Database={_config.DatabaseName};Uid={_config.Username};Pwd={_config.Password};DefaultCommandTimeout=60;";
            return new MySqlConnection(connStr);
        }
    }
}
