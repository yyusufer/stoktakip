namespace Takip.Models
{
    public class DatabaseConfigViewModel
    {
        public string? HostType { get; set; }
        public string? HostIp { get; set; }
        public string? Port { get; set; } 
        public string? Username { get; set; } 
        public string? Password { get; set; }
        public string DatabaseName { get; set; } = "stoktakipdb";
    }
}
