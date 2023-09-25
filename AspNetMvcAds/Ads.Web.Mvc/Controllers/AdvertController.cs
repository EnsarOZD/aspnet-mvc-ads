using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
        [Route("/advert/search")]
        public IActionResult Search(int page, string query)
        {
            ViewData["ListPartialTitle"] = query ?? "";
            return View();
        }

        [Route("/advert/{title-slug}")]
        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}
