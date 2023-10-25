using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	public class AdvertCommentController : Controller
	{
		[Area("Admin")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
