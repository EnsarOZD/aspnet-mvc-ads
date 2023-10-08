using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Controllers
{
    public class CategoryController : Controller
    {
        private AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [Route("/category/")]
        [Route("/category/{category-slug}")]
        public IActionResult Index(int id, int page)
        {
            ViewData["CategoryId"] = id;
            ViewData["Page"] = page;

            int skipCount = (page - 1) * 10;

            var category = _context.CategoryEntities.FirstOrDefault(x => x.Id == id);

            if (category != null)
            {
                ViewData["ListPartialTitle"] = category.Name;

                var advertIds = _context.CategoryAdvertEntities
                    .Where(x => x.CategoryId == category.Id)
                    .Select(x => x.AdvertId)
                    .ToList();

                if (advertIds.Any())
                {
                    var advertEntityList = _context.AdvertEntities.Where(x => advertIds.Contains(x.Id)).Skip(skipCount).Take(10).ToList();

                    var photoEntityList = _context.AdvertImageEntities.Where(x => advertIds.Contains(x.AdvertId)).ToList();

                    var advertModelList = advertEntityList.Select(x => new AdvertViewModel
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Description = x.Description,
                        CreatedAt = x.CreatedAt,
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        ImagePath = photoEntityList.FirstOrDefault(p => p.AdvertId == x.Id)?.ImagePath ?? ""
                    }).ToList();

                    return View(advertModelList);
                }
            }

            ViewData["ListPartialTitle"] = "No_Category";
            return View();
        }
    }
}
