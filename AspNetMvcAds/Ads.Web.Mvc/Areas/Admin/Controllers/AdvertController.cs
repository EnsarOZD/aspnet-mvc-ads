using Ads.Data;
using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;

        public AdvertController(
            IRepository<AdvertEntity> advertRepository,
            IAdvertImageService advertImageService,
            IFileService fileService,
            AppDbContext context)
        {
            _advertRepository = advertRepository;
            _advertImageService = advertImageService;
            _fileService = fileService;
            _context = context;
        }

        public async Task<IActionResult> Index(string searchContent, int? page)
        {
            int pageSize = 10;
            var adverts = await _advertRepository.GetAll().ToListAsync();
            var advertImages = _advertImageService.GetAllImages().ToList();
            var advertViewModels = adverts.Select(advert => new AdvertViewModel
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
            }).ToList();

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
            var operationResult = await _advertRepository.GetById(id);
            if (!operationResult.Success || operationResult.Data == null)
            {
                TempData["ErrorMessage"] = operationResult.ErrorMessage ?? "Advert not found";
                return RedirectToAction("Index");
            }

            var deleteResult = await _advertRepository.Delete(operationResult.Data);

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
                ImagePath = advert.ImagePath
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdvertViewModel viewModel, IFormFile? uploadedImage)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var operationResult = await _advertRepository.GetById(viewModel.Id);
            if (!operationResult.Success || operationResult.Data == null)
            {
                TempData["ErrorMessage"] = "Advert not found";
                return RedirectToAction("Index");
            }

            var advert = operationResult.Data;

            // Eski resimleri sil
            var oldImages = _advertImageService.GetAllImagesQueryable().Where(img => img.AdvertId == advert.Id).ToList();
            foreach (var oldImage in oldImages)
            {
                _advertImageService.DeleteImage(oldImage.Id);
            }

            if (uploadedImage != null && uploadedImage.Length > 0)
            {
                if (uploadedImage.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("UploadedImage", "Dosya boyutu 2 MB'dan büyük olamaz.");
                    return View(viewModel);
                }

                if (Path.GetExtension(uploadedImage.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("UploadedImage", "Sadece .jpg uzantılı dosyaları yükleyebilirsiniz.");
                    return View(viewModel);
                }

                await _fileService.UploadFileAsync(uploadedImage);
                string imageName = uploadedImage.FileName;

                string imagePath = $"/uploads/{imageName}";
                advert.ImagePath = imagePath;

                ModelState.Remove("UploadedImage");
            }

            advert.Title = viewModel.Title;
            advert.Description = viewModel.Description;
            advert.Price = viewModel.Price;
            advert.AdvertClickCount = viewModel.AdvertClickCount;

            var updateResult = await _advertRepository.Update(advert);
            if (updateResult.Success)
            {
                TempData["SuccessMessage"] = "Advert updated successfully";

                // Yeni resimleri ekle
                if (uploadedImage != null)
                {
                    var newAdvertImage = new AdvertImageEntity
                    {
                        AdvertId = advert.Id,
                        ImagePath = advert.ImagePath
                    };

                    _advertImageService.AddImage(newAdvertImage);
                }
            }
            else
            {
                TempData["ErrorMessage"] = updateResult.ErrorMessage;
            }

            return RedirectToAction("Index");
        }

    }
}
