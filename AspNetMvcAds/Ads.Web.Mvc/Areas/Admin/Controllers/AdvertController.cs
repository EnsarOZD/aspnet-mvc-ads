using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var adverts = _context.AdvertEntities.Select(advert => new AdvertViewModel
            {
                Id = advert.Id,
                AdvertClickCount = advert.AdvertClickCount,
                Description = advert.Description,
                Price = advert.Price,
                Title = advert.Title,
                UserId = advert.UserId,
            }).ToList();
            return View(adverts);
        }
        [Area("Admin")]
        public IActionResult Delete()
        {

            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            DeleteAdvert(id);
            return RedirectToAction("Index");
        }
        public void DeleteAdvert(int commentId)
        {
            //var comment = _context.AdvertCommentEntities.FirstOrDefault(c => c.Id == commentId);
            var comment = _context.AdvertEntities.Find(commentId);
            if (comment != null)
            {
                _context.AdvertEntities.Remove(comment);
                _context.SaveChanges();
                ViewBag.SuccessMessage = "Advertisement has been deleted successfully.";
            }
    }
}
}
