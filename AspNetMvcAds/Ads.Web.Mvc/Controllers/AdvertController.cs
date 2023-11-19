using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertController : Controller
    {
        private readonly IRepository<AdvertEntity> _advertRepository;
        private readonly IAdvertImageService _advertImageService;
        private readonly ISearchService _searchService;
        private readonly IFileService _fileService;

        public AdvertController(IRepository<AdvertEntity> advertRepository,
                                IAdvertImageService advertImageService,
                                ISearchService searchService,
                                IFileService fileService)
        {
            _advertRepository = advertRepository;
            _advertImageService = advertImageService;
            _fileService = fileService;


        }

        public async Task<IActionResult> Index(string searchContent, int? page)
        {
            int pageSize = 10;
            var adverts = await _advertRepository.GetAll().ToListAsync();
            var advertImages = _advertImageService.GetAllImages().ToList();
            var advertViewModels = new List<AdvertViewModel>();

            foreach (var advert in adverts)
            {
                var viewModel = new AdvertViewModel
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

            int pageIndex = page ?? 1;
            var pagedAdverts = advertViewModels.ToPagedList(pageIndex, pageSize);
            return View(pagedAdverts);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var operationsResult = await _advertRepository.GetById(id);
            if (!operationsResult.Success || operationsResult.Data == null)
            {
                TempData["ErrorMessage"] = operationsResult.ErrorMessage ?? "Advert not found";
                return RedirectToAction("Index");
            }

            var deleteResult = await _advertRepository.Delete(operationsResult.Data);

            TempData["SuccessMessage"] = deleteResult.Success ? "Advert deleted successfully" : deleteResult.ErrorMessage;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var operationResult = await _advertRepository.GetById(id);
            if (!operationResult.Success || operationResult.Data == null)
            {
                TempData["ErrorMessage"] = "Advert not found";
                return RedirectToAction("Index");
            }

            var advert = operationResult.Data;
            var viewModel = new AdvertViewModel
            {
                Id = advert.Id,
                Title = advert.Title,
                Description = advert.Description,
                Price = advert.Price,
                AdvertClickCount = advert.AdvertClickCount,
                UserId = advert.UserId,
                ImagePath = advert.ImagePath
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdvertViewModel viewModel, int id, IFormFile uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var operationResult = await _advertRepository.GetById(id);
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
                    ModelState.AddModelError("File", "Sadece .jpg uzantılı dosyaları yükleyebilirsiniz.");
                    return View(viewModel);
                }

                await _fileService.UploadFileAsync(uploadedImage);
                string imageName = uploadedImage.FileName;

                advert.ImagePath = $"/uploads/{imageName}"; //TODO: Tilde silindi
            }

            advert.Title = viewModel.Title;
            advert.Description = viewModel.Description;
            advert.Price = viewModel.Price;
            advert.AdvertClickCount = viewModel.AdvertClickCount;

            var updateResult = await _advertRepository.Update(advert);

            if (updateResult.Success)
            {
                TempData["SuccessMessage"] = "Advert updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = updateResult.ErrorMessage;
            }

            return RedirectToAction("Index");
        }
    }
}
