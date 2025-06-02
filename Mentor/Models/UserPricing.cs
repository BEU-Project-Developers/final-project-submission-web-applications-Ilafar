using System.ComponentModel.DataAnnotations;

namespace Mentor.Models
{
    public class UserPricing
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // FK to Identity User

        public int PricingId { get; set; } // Selected pricing

        public bool IsFeatured { get; set; } // Only one true per user
    }

}
