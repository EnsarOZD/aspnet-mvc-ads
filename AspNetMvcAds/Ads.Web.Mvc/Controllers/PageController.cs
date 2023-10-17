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
        [Route("/page/{title-slug}")]
        public IActionResult Detail(int id)
        {
            var page=_db.PageEntities.FirstOrDefault(x => x.Id == id);

            if (page==null)
            {
                return NotFound();
            }
           
            return View(page);
        }
    }
}
