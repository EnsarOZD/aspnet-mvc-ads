using Ads.Data;
using Ads.Data.Entities;
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
        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
           
            var adverts = await _context.AdvertEntities
                .Select(advert => new AdvertViewModel
            {
                Id = advert.Id,
                AdvertClickCount = advert.AdvertClickCount,
                Description = advert.Description,
                Price = advert.Price,
                Title = advert.Title,
                UserId = advert.UserId,
                ImagePath= _context.AdvertImageEntities.First().ImagePath,
                }).ToListAsync();
            return View(adverts);
        }


        [Area("Admin")]
        public IActionResult Delete()
        {

            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await DeleteAdvertAsync(id);
            TempData["SuccessMessage"] = "Advert deleted succesfully";
            return RedirectToAction("Index");
        }
        public async Task DeleteAdvertAsync(int commentId)
        {
            //var comment = _context.AdvertCommentEntities.FirstOrDefault(c => c.Id == commentId);
            var comment = _context.AdvertEntities.Find(commentId);
            if (comment != null)
            {
                _context.AdvertEntities.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
