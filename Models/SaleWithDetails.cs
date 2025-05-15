namespace Takip.Models
{
    public class SaleWithDetails
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? PersonnelName { get; set; }
        public DateTime Time { get; set; }
        public decimal Price { get; set; }
    }
}
