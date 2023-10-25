using Ads.Data.Entities;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdvertSearchViewModel
    {
        public List<AdvertEntity> AdvertEntities { get; set; }
        public List<AdvertImageEntity> AdvertImageEntities { get; set; }
        public List<CategoryEntity> Categories { get; set; }
        public List<CategoryAdvertEntity> CategoryAdvertEntities { get; set; }
    }
}
