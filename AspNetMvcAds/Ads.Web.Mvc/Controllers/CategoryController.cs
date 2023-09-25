using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        [Route("/category/")]
        [Route("/category/{category-slug}")]
        public IActionResult Index(int id, int page)
        {
            ViewData["ListPartialTitle"] = "Electronics";
            return View();
        }
    }
}
