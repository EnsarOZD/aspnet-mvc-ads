using Ads.Data.Entities;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdminAdvertImageViewModel
    {
        public int Id { get; set; }
        public int AdvertId { get; set; }
        public string AdvertTitle { get; set; } = string.Empty;
        public long? ImageSize { get; set; }
        public string? ImagePath { get; set; } = string.Empty;
        public IEnumerable<AdvertImageEntity> AdvertImages;
    }
}
