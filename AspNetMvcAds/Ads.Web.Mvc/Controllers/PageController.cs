using Ads.Data;
using Ads.Data.Entities;
using Ads.Web.Mvc.Areas.Admin.Controllers;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;
using System.Security.Claims;

namespace Ads.Web.Mvc.Controllers
{
    public class PageController : Controller
    {
        private readonly AppDbContext _db;

        public PageController(AppDbContext db)
        {
            _db = db;
        }



        [Route("/page")]
        [Route("/page/{slug}")]
        public IActionResult Detail(string slug)
        {
            var page = _db.PageEntities.FirstOrDefault(x => x.Slug == slug);

            if (page == null)
            {
                return NotFound();
            }

            return View(slug, page);
        }
        [Route("/page")]
        [Route("/page/{slug}")]
        [HttpPost]
        public IActionResult ContactMessages(ContactUsMessagesEntity messages)
        {
            if (ModelState.IsValid)
            {
                messages.DateTime = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                messages.Status = true;
                _db.Add(messages);
                _db.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("/page/package")]
        [HttpGet]
        public IActionResult Package()
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var user = _db.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            TempData["UserRole"] = user.Roles;
            return View();
        }


        [Route("/page/package")]
        [HttpPost]
        public IActionResult PackageSend(UserRoles roles)
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var user = _db.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            user.Roles = roles.ToString();
            _db.UserEntities.Update(user);
            _db.SaveChanges();

            return Redirect("/page/package");
        }
    }
}
