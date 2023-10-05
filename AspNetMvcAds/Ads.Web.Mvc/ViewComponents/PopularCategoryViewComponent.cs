using Ads.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.ViewComponents
{
	public class PopularCategoryViewComponent : ViewComponent
	{
		private readonly AppDbContext _db;

		public PopularCategoryViewComponent(AppDbContext db)
		{
			_db = db;
		}


		public async Task<IViewComponentResult>  InvokeAsync()
		{
			var categoryAdverts=await _db.CategoryAdvertEntities
				.Include(ca=>ca.Category)
				.Include(ca=>ca.Advert)
				.ToListAsync();

			var popularCategories = categoryAdverts
				.GroupBy(ca=>ca.CategoryId)
				.Select(group => new
			{
				Category = group.First().Category,
				TotalClicks = group.Sum(ca => ca.Advert.AdvertClickCount)

			})
			.OrderByDescending(x => x.TotalClicks)
			.Take(5)
			.Select(x => x.Category)
			.ToList();

			return View(popularCategories);
		}
	}
}
