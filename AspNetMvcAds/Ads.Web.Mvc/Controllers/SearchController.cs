using Ads.Data;
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
        public IActionResult Index(string searchContent)
        {
            var filteredTitle = from s in _context.AdvertEntities
                                select s;
            var filteredPosts = from s in _context.CategoryEntities
                                select s;
            if (!string.IsNullOrEmpty(searchContent))
            {
                filteredTitle = filteredTitle.Where(p => p.Title.Contains(searchContent));
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
