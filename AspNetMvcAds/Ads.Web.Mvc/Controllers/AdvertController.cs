using Ads.Data;
using Ads.Web.Mvc.Models;
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

        [Route("/advert/detail/{id}")]
        public IActionResult Detail(int id)
        {
            var advertEntity = _db.AdvertEntities.FirstOrDefault(x => x.Id == id);
            if (advertEntity != null)
            {
                var categoryId = _db.CategoryAdvertEntities.FirstOrDefault(x => x.AdvertId == id)?.CategoryId ?? 0;
                var category = _db.CategoryEntities.FirstOrDefault(x => x.Id == categoryId);

                var user = _db.UserEntities.FirstOrDefault(x => x.Id == advertEntity.UserId);

                var imagePaths = _db.AdvertImageEntities.Where(x => x.AdvertId == id).Select(x => x.ImagePath).ToList();

                var advertModel = new AdvertDetailViewModel
                {
                    Id = advertEntity.Id,
                    Title = advertEntity.Title,
                    Description = advertEntity.Description,
                    CreatedAt = advertEntity.CreatedAt,
                    CategoryId = category?.Id ?? 0,
                    CategoryName = category?.Name ?? "-",
                    ImagePaths = imagePaths,
                    User = new UserViewModel
                    {
                        Id = user.Id,
                        Name = user.Name,
                        CreatedAt = user.CreatedAt
                    }
                };

                return View(advertModel);
            }

            return View();
        }
    }
}
