namespace aptechvision.Models
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } // Ensure this property exists
        public decimal PricePerKg { get; set; }
    }
}
