using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login(string redirectUrl)
        {
            return Redirect(redirectUrl);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
