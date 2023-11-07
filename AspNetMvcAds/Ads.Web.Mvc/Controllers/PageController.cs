using Ads.Data;
using Ads.Data.Entities;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;

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
            var page=_db.PageEntities.FirstOrDefault(x => x.Slug == slug);

            if (page==null)
            {
                return NotFound();
            }
           
            return View(slug,page);
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



    }
}
