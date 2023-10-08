using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
        [Route("/page")]
        [Route("/page/{title-slug}")]
        public IActionResult Detail(int id)
        {
            return View();
        }
        [HttpGet("/page/SingleDetail")]
        public IActionResult SingleDetail()
        {
            return View();
        }
    }
}
