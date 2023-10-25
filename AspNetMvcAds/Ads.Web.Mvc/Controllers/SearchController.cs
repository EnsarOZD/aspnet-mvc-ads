using Ads.Data;
using Ads.Data.Entities;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        public SearchController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchContent, int? categoryId, string location)
        {
            IQueryable<AdvertEntity> filteredTitle = _context.AdvertEntities;

            IQueryable<CategoryEntity> filteredPosts = _context.CategoryEntities;


            if (!string.IsNullOrEmpty(searchContent))
            {
                filteredTitle = filteredTitle.Where(p => p.Title.Contains(searchContent));
            }

            if (categoryId is not null)
            {
                var alakaliReklamIdleri = _context.CategoryAdvertEntities
                    .Where(x => x.CategoryId == categoryId)
                    .Select(x => x.AdvertId)
                    .ToList();

                filteredTitle = filteredTitle.Where(x => alakaliReklamIdleri.Contains(x.Id));
            }





            var titles = filteredTitle.ToList();
            var categories = filteredPosts.ToList();
            var viewModel = new SearchViewModel
            {
                Titles = titles,
                Categories = categories,
            };
            return View(viewModel);
        }
    }
}
