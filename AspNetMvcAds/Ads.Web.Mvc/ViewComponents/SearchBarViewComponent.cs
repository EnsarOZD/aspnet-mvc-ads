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

        public IViewComponentResult Invoke(int id, string searchContent)
        {
            var viewModel = new SearchViewModel();

            if (!string.IsNullOrEmpty(searchContent))
            {

                var titles = _context.CategoryAdvertEntities
                    .Where(ca => ca.CategoryId == id)
                    .Select(ca => ca.Advert)
                    .Where(a => a.Title.Contains(searchContent))
                    .ToList();


                var category = _context.CategoryEntities.FirstOrDefault(c => c.Id == id);
                if (category != null)
                {
                    viewModel.CategoryEntities = new List<CategoryEntity> { category };
                }

                viewModel.AdvertEntities = titles;
            }
            else
            {

                viewModel.AdvertEntities = _context.AdvertEntities.ToList();
                viewModel.CategoryEntities = _context.CategoryEntities.ToList();
            }

            return View(viewModel);
        }


    }
}
