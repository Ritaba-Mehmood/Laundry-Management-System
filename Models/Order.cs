namespace aptechvision.Models
{
    public class Order
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime PickupDate { get; set; }
        public string Slot { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PackageId { get; set; } // Ensure this property exists

        public DateTime CreatedAt { get; set; }
        public Package Package { get; set; } // Navigation property
    }

}
