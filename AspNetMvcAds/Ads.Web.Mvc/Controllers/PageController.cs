using Ads.Data;
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

       
    }
}
