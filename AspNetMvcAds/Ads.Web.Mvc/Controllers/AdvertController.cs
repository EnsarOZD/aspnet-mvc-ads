using Ads.Data;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
		private readonly AppDbContext _db;

		public AdvertController(AppDbContext db)
		{
			_db = db;
		}


		[Route("/advert/search")]
        public IActionResult Search(int page, string query)
        {
            ViewData["ListPartialTitle"] = query ?? "";
            return View();
        }

        [Route("/advert/{title-slug}")]
        public IActionResult Detail(int id)
        {
         
            var advert=_db.AdvertEntities.FirstOrDefault(x => x.Id == id);
            if (advert!=null)
            {
                advert.AdvertClickCount++;
                _db.SaveChanges();
            }
            return View();
        }
    }
}
