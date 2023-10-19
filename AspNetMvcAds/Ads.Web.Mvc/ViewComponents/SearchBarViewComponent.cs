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

		public IViewComponentResult Invoke(string searchContent)
		{
			// Burada veritabanından kategori listesini alabilirsiniz. Örneğin:
			var adverts = new SearchViewModel
			{

				Titles = _context.AdvertEntities.ToList()
			};
			var categoriess = new CategoryViewModel
			{
				Categories = _context.CategoryEntities.ToList()
			};
			var filteredTitle = from s in _context.AdvertEntities
								select s;
			var filteredCategories = from s in _context.CategoryEntities
									 select s;
			if (!string.IsNullOrEmpty(searchContent))
			{
				filteredTitle = filteredTitle.Where(p => p.Title.Contains(searchContent));
				filteredCategories = filteredCategories.Where(p => p.Name.Contains(searchContent));
			}
			var titles = filteredTitle.ToList();
			var categories = filteredCategories.ToList();
			var viewModel = new SearchViewModel
			{
				Titles = titles,
				Categories = categories,
			};

			return View(viewModel);
		}
	}
}
