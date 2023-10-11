using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
	public class AddListingController : Controller
	{
		private readonly AppDbContext _context;

		public AddListingController(AppDbContext context)
		{
			_context = context;
		}
		[HttpGet]
		public IActionResult Index()
		{
			
			ViewData["Categories"] = _context.CategoryEntities.ToList();

			
			var model = new AddListingViewModel();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Index(AddListingViewModel model)
		{
			if (ModelState.IsValid)
			{
				var advert = model.Advert;
				var category = model.Category;
				var user = model.User;
				var advertImages = model.AdvertImages;

				_context.AdvertEntities.Add(advert);
				await _context.SaveChangesAsync();

				if (advertImages != null && advertImages.Any())
				{
					foreach (var image in advertImages)
					{
						image.AdvertId = advert.Id;
						_context.AdvertImageEntities.Add(image);
					}
					await _context.SaveChangesAsync();
				}

				return RedirectToAction("Success");
			}

			return View(model);
		}

	}
}

