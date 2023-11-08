using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Policy = "RequireAdministratorRole")] // TODO:Yetkilendirmelerde açılacak.
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
