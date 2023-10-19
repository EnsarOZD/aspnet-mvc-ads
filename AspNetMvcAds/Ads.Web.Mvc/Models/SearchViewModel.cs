using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
    public class SearchViewModel
    {
        public List<AdvertEntity> Titles { get; set; }
        public List<CategoryEntity> Categories { get; set; }
        //public List<CategoryViewModel> Categories { get; set; }
    }
}
