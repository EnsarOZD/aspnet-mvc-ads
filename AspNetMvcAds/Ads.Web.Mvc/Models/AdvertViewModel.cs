using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
    public class AdvertViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public string AdFeature { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public List<int> CategoryIds { get; set; } = new List<int>();
		public List<string> CategoryNames { get; set; } = new List<string>();
        public object AdvertClickCount { get; internal set; }
    }
}
