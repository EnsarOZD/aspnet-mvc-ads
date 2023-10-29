using Ads.Data;
using Ads.Data.Entities;
using Ads.Services.Services;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Models;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdvertImageController : Controller
	{
		private readonly IAdvertImageService _advertImageService;
        //private readonly AppDbContext _context;
        private readonly ISearchService _searchService;

        public AdvertImageController(IAdvertImageService advertImageService, AppDbContext context, ISearchService searchService)
		{
			_advertImageService = advertImageService;
            //_context = context;
            _searchService = searchService;
        }
		public IActionResult Index(string searchContent)
		{
            var searchResult = _searchService.SearchAdvertsByTitle(searchContent);
            var viewModel = new AdvertViewModel
            {
                AdvertEntities = searchResult.AdvertEntities.ToList(),
                AdvertImageEntities = searchResult.AdvertImageEntities.ToList(),
            };

            //IEnumerable<AdvertImageEntity> images;
            //if (advertId.HasValue)
            //{
            //    images = _advertImageService.GetAllImages().Where(i => i.AdvertId == advertId.Value);


            //}
            //else
            //{
            //    images=_advertImageService.GetAllImages();
            //}

            return View(viewModel);
        }
        public IActionResult Delete(int id)
        {
            var image = _advertImageService.GetImageById(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _advertImageService.DeleteImage(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var advertImage=_advertImageService.GetImageById(id);
            if (advertImage == null)
            {
                return NotFound();
            }
            var model = new AdminAdvertImageViewModel
            {
                AdvertId = advertImage.Id,
                ImagePath = advertImage.ImagePath,
                ImageSize = advertImage.ImageSize,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AdminAdvertImageViewModel entity)
        {
           
            if (ModelState.IsValid)
            {
                var advertImage = _advertImageService.GetImageById(entity.Id);
                if (advertImage==null)
                {
                    return NotFound();
                }
                advertImage.ImagePath = entity.ImagePath;
                _advertImageService.UpdateImage(advertImage);
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }
    }
}
