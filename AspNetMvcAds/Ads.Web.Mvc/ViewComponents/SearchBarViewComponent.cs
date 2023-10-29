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

		public IViewComponentResult Invoke(int id, string searchContent)
		{
			var viewModel = new SearchViewModel();

			if (!string.IsNullOrEmpty(searchContent))
			{
				// Kategoriye ait başlıkları sorgula
				var titles = _context.CategoryAdvertEntities
					.Where(ca => ca.CategoryId == id) // Kategoriye göre filtrele
					.Select(ca => ca.Advert) // İlanlara eriş
					.Where(a => a.Title.Contains(searchContent)) // Başlıkları ara
					.ToList();

				viewModel.Titles = titles;
			}
			else
			{
				// Eğer kategori seçimi yoksa, tüm başlıkları getir
				viewModel.Titles = _context.AdvertEntities.ToList();
			}

			// Diğer gerekli verileri de model içerisine ekleyebilirsiniz
			viewModel.Categories = _context.CategoryEntities.ToList();

			return View(viewModel);
		}

	}
}
