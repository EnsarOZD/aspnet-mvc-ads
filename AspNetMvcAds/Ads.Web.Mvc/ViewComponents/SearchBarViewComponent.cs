using Ads.Data;
using Ads.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ads.Web.Mvc.Models;
namespace Ads.Web.Mvc.ViewComponents
{
    public class SearchBarViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public SearchBarViewComponent(AppDbContext context)
        {
            _context = context;
        }

        //public async Task<IViewComponentResult> InvokeAsync(string searchQuery)
        //{
        //    //var articles = _context.AdvertEntities.AsQueryable();

        //    //if (!string.IsNullOrEmpty(searchQuery))
        //    //{
        //    //    articles = articles.Where(a => a.Title.Contains(searchQuery));
        //    //}

        //    //return View(articles.ToList());
        //    return View();
        //}

        public IViewComponentResult Invoke(string searchText)
        {
            // Burada veritabanından kategori listesini alabilirsiniz. Örneğin:
            var categories = new CategoryViewModel
            {
                Categories = _context.CategoryEntities.ToList()
            };
                               

            return View(categories);
        }
    }
}
