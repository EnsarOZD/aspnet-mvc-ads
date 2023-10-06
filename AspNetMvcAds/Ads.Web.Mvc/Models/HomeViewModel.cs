namespace Ads.Web.Mvc.Models
{
	public class HomeViewModel
	{
        public HomeViewModel()
        {
			Adverts = new List<AdvertViewModel>();
			Categories = new List<CategoryViewModel>();
		}

        public List<AdvertViewModel> Adverts { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }
}
