using System;
using System.ComponentModel.DataAnnotations;

namespace aptechvision.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime PickupDate { get; set; }
        public string Slot { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int PackageId { get; set; } // Ensure this property exists
        public Package Package { get; set; } // Navigation property
    }
}
