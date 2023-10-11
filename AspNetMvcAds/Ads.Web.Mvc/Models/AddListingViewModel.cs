using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
	public class AddListingViewModel
	{
		public AdvertEntity Advert { get; set; }
		public List<AdvertImageEntity>? AdvertImages { get; set; }
		public UserEntity? User { get; set; }
		public CategoryEntity Category { get; set; }
	}
}
