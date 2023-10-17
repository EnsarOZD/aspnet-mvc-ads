using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
    public class PopularDealViewModel
    {
        public List<AdvertImageEntity> AdvertImageEntities { get; set; }
        public List<AdvertEntity> AdvertEntities { get; set; }
		public List<CategoryEntity> CategoryEntities { get; set; }
        public List<CategoryAdvertEntity> CategoryAdvertEntities { get; set; }
    }
}
