using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
