using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ads.Web.Mvc.ViewComponents
{
    public class TrendingAddsViewComponent : ViewComponent
    {
        public ViewViewComponentResult Invoke()
        {

            return View();
        }
    }
}
