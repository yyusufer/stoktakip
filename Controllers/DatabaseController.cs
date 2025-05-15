using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text.Json;
using Takip.Models;

namespace Takip.Controllers
{
    public class DatabaseController : Controller
    {
        private readonly string configPath = Path.Combine(Directory.GetCurrentDirectory(), "connection.json");

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TestConnection(string serverType, string ip, string port, string username, string password)
        {
            string host = serverType == "ip" ? ip : "localhost";
            string connectionString = $"Server={host};Port={port};Database=stoktakipdb;Uid={username};Pwd={password};";

            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    return Json(new { success = true, message = "Bağlantı başarılı!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Bağlantı başarısız: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SaveConnection(string serverType, string ip, string port, string username, string password)
        {
            var connectionInfo = new DatabaseConfigViewModel
            {
                HostType = serverType,
                HostIp = ip,
                Port = port,
                Username = username,
                Password = password,
                DatabaseName = "stoktakipdb"
            };

            var jsonString = JsonSerializer.Serialize(connectionInfo, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(configPath, jsonString);

            return Json(new { success = true, message = "Veritabanı bağlantı bilgileri kaydedildi!" });
        }

        [HttpGet]
        public IActionResult GetSavedConnectionInfo()
        {
            if (!System.IO.File.Exists(configPath))
                return Json(new { exists = false });

            try
            {
                var json = System.IO.File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<DatabaseConfigViewModel>(json);

                if (config == null)
                    return Json(new { exists = false });

                string? hostText = config.HostType == "ip" ? config.HostIp : "localhost (yerel ağ)";

                return Json(new
                {
                    exists = true,
                    host = hostText,
                    username = config.Username
                });
            }
            catch
            {
                return Json(new { exists = false });
            }
        }

        [HttpPost]
        public IActionResult DeleteConnectionFile()
        {
            try
            {
                if (System.IO.File.Exists(configPath))
                    System.IO.File.Delete(configPath);

                return Json(new { success = true, message = "Bağlantı bilgileri silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silinemedi: " + ex.Message });
            }
        }
    }
}
