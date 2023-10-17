using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
	public class CategoryViewModel
	{
		
		public int Id { get; set; }
        public string Name { get; set; }
		public List<CategoryEntity> Categories { get; set; }
    }
}
