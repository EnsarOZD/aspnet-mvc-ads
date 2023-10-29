using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ads.Web.Mvc.ViewComponents
{
    //[ViewComponent(Name = "Home")]
    public class PopularDealViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public PopularDealViewComponent(AppDbContext db)
        {
            _db = db;
        }


        public ViewViewComponentResult Invoke()
        {
            var advertImage = new PopularDealViewModel
            {
                AdvertImageEntities = _db.AdvertImageEntities.ToList(),
                AdvertEntities = _db.AdvertEntities.ToList(),
                CategoryEntities = _db.CategoryEntities.ToList(),
                CategoryAdvertEntities = _db.CategoryAdvertEntities.ToList()


            };

            return View(advertImage);
        }
    }
}
