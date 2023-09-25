using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
        public IActionResult Search(int page, string query)
        {
            return View();
        }

        [Route("/advert/")]
        [Route("/advert/{title-slug}")]
        public IActionResult Detail(int id)
        {
            return View();
        }
    }
}
