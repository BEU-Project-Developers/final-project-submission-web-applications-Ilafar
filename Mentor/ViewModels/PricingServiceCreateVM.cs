using Mentor.Models;

namespace Mentor.ViewModels
{
    public class PricingServiceCreateVM
    {
        public int PricingId { get; set; }
        public Pricing Pricing { get; set; }
        public List<int> ServiceIds { get; set; }
    }
}
