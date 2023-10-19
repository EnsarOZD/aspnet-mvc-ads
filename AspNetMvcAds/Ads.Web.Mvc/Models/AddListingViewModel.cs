using Ads.Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ads.Web.Mvc.Models
{
    public class AddListingViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string AdType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsPriceNegotiable { get; set; }
        public IFormFile? File { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string AddFeature { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactAddress { get; set; } = string.Empty;
        public string AdFeature { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public bool AcceptTermsAndConditions { get; set; }
        public AdvertEntity? Advert { get; set; }
        public UserEntity? User { get; set; }
        [ForeignKey("CategoryName")]
        public CategoryEntity? Category { get; set; }
    }
}
