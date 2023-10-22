using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdvertImageController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
