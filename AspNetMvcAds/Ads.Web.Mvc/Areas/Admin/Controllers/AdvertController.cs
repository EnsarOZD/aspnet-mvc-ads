﻿using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AdvertController : Controller
	{
		private readonly IRepository<AdvertEntity> _advertReposityory;
		private readonly IAdvertImageService _advertImageService;
		private readonly ISearchService _searchService;
		private readonly IFileService _fileService;

		public AdvertController(IRepository<AdvertEntity> advertReposityory,
								IAdvertImageService advertImageService,
								ISearchService searchService,
								IFileService fileService)
		{
			_advertReposityory = advertReposityory;
			_advertImageService = advertImageService;
			_searchService = searchService;
			_fileService = fileService;
		}

		public async Task<IActionResult> Index(string searchContent)
		{
			var adverts = await _advertReposityory.GetAll().ToListAsync();
            var advertImages = _advertImageService.GetAllImages().ToList();
            var advertViewModels=new List<AdvertViewModel>();

			foreach (var advert in adverts)
			{
				var viewModel=new AdvertViewModel
				{
					Id = advert.Id,
					Title = advert.Title,
					Description = advert.Description,
					Price = advert.Price,
					AdvertClickCount = advert.AdvertClickCount,
					UserId = advert.UserId,
                    ImagePaths = advertImages
                    .Where(img => img.AdvertId == advert.Id)
                    .Select(img => img.ImagePath)
                    .ToList(),
                };
				advertViewModels.Add(viewModel);
			}

			if (!string.IsNullOrEmpty(searchContent))
			{
				advertViewModels = advertViewModels.Where(i => i.Title.Contains(searchContent)).ToList();
			}

			return View(advertViewModels);
		}
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			var operationsResult = await _advertReposityory.GetById(id);
			if (!operationsResult.Success || operationsResult.Data == null)
			{
				TempData["ErrorMessage"] = operationsResult.ErrorMessage ?? "Advert not found";
				return RedirectToAction("Index");
			}
			
			var deleteResult = await _advertReposityory.Delete(operationsResult.Data);

			TempData["SuccesMessage"] = deleteResult.Success ? "Advert deleted successfully" : deleteResult.ErrorMessage;
			if (deleteResult.Success)
			{
				TempData["SuccessMessage"] = "Advert deleted successfully";
			}
			else
			{
				TempData["ErrorMessage"] = deleteResult.ErrorMessage;
			}
			return RedirectToAction("Index");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			var operaationResult = await _advertReposityory.GetById(id);
			if (!operaationResult.Success || operaationResult.Data == null)
			{
				TempData["ErrorMessage"] = "Advert not found";
				return RedirectToAction("Index");
			}

			var advert = operaationResult.Data;
			var viewModel = new AdvertViewModel
			{
				Id = advert.Id,
				Title = advert.Title,
				Description = advert.Description,
				Price = advert.Price,
				AdvertClickCount = advert.AdvertClickCount,
				//UserId = advert.UserId,
				ImagePath = advert.ImagePath

			};
			return View(viewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(AdvertViewModel viewModel, int id, IFormFile? uploadedImage)
		{

			if (!ModelState.IsValid)
			{
				return View(viewModel);
			}

			var operationResult = await _advertReposityory.GetById(viewModel.Id);
			if (!operationResult.Success || operationResult.Data == null)
			{
				TempData["ErrorMessage"] = "Advert not found";
				return RedirectToAction("Index");
			}
			var advert = operationResult.Data;

			if (uploadedImage != null && uploadedImage.Length > 0)
			{
				if (uploadedImage.Length > 2 * 1024 * 1024)
				{
					ModelState.AddModelError("File", "Dosya boyutu 2 MB'dan büyük olamaz.");
					return View(viewModel);

				}

				if (Path.GetExtension(uploadedImage.FileName).ToLower() != ".jpg")
				{
					ModelState.AddModelError("File", "Sadece .jpg uzanıtılı dosyaları yükleyebilirsiniz.");
					return View(viewModel);
				}

				await _fileService.UploadFileAsync(uploadedImage);
				string imageName = uploadedImage.FileName;
				


				advert.ImagePath = $"/uploads/{imageName}"; //TODO: Tilde silindi
				if (uploadedImage != null)
				{
					ModelState.Remove("File");
				}

			}
			advert.Title = viewModel.Title;
			advert.Description = viewModel.Description;
			advert.Price = viewModel.Price;
			advert.AdvertClickCount = viewModel.AdvertClickCount;
			//advert.UserId=viewModel.UserId;

			var updateResult = await _advertReposityory.Update(advert);
			if (updateResult.Success)
			{
				if (uploadedImage!=null)
				{
					var advertImage = _advertImageService.GetImageById(advert.Id);
					advertImage.ImagePath = advert.ImagePath;
					_advertImageService.UpdateImage(advertImage);
				}
				
				TempData["SuccessMessage"] = "Advert and images updated successfully";
			}
			else
			{
				TempData["ErrorMessage"] = updateResult.ErrorMessage;
			}
		
			
            return RedirectToAction("Index");
	}
}
}
