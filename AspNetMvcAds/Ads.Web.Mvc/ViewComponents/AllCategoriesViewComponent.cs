using Ads.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.ViewComponents
{
    public class AllCategoriesViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public AllCategoriesViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _db.CategoryEntities
                .ToListAsync();


            return View(categories);
        }
    }
}
