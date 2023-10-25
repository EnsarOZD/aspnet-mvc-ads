using Ads.Data.Entities;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdvertSearchViewModel
    {
        public List<AdvertEntity>? Titles { get; set; }
        public List<AdvertImageEntity>? AdvertImages { get; set; }
        public List<CategoryEntity>? Categories { get; set; }
    }
}
