using Ads.Data;
using Ads.Data.Entities;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ads.Web.Mvc.Controllers
{
    public class AddListingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        public AddListingController(IFileService fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }
        public IActionResult Add()
        {
            var categories = _context.CategoryEntities.ToList();
            ViewData["Categories"] = new SelectList(categories, nameof(CategoryEntity.Id), nameof(CategoryEntity.Name));
            var model = new AddListingViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddListingViewModel model, [FromForm] IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Dosya boyutu 2 MB'dan büyük olamaz.");
                    ViewBag.Error = "Dosya boyutu 2 MB'dan büyük olamaz.";
                    return View("Add", model);
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("File", "Sadece .jpg uzantılı dosyaları yükleyebilirsiniz.");
                    ViewBag.Error = "Sadece.jpg uzantılı dosyaları yükleyebilirsiniz.";
                    return View("Add", model);
                }
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            await _fileService.UploadFileAsync(formFile);
            ViewBag.SuccessMessage = "Uploaded Successfully";


            var categories = _context.CategoryEntities.ToList();
            ViewData["Categories"] = new SelectList(categories, nameof(CategoryEntity.Id), nameof(CategoryEntity.Name));

            if (!int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int userId))
            {
                return BadRequest();
            }

            AdvertEntity newAdvert = new()
            {
                AdType = model.AdType,
                AcceptTermsAndConditions = model.AcceptTermsAndConditions,
                AdFeature = model.AdFeature,
                AdvertClickCount = 0,
                ContactEmail = model.ContactEmail,
                ContactAddress = model.ContactAddress,
                ContactName = model.ContactName,
                ContactNumber = model.ContactNumber,
                CreatedAt = DateTime.Now,
                DeletedAt = DateTime.Now,
                Description = model.Description,
                IsPriceNegotiable = model.IsPriceNegotiable,
                PaymentMethod = model.PaymentMethod,
                Price = model.Price,
                Title = model.Title,
                UpdatedAt = DateTime.Now,
                UserId = userId,
            };
            _context.AdvertEntities.Add(newAdvert);
            await _context.SaveChangesAsync();
            string imageName = formFile.FileName;
            string imagePath = $"~/uploads/{imageName}";
            long imageSize = formFile.Length;
            AdvertImageEntity advImage = new()
            {
                AdvertId = newAdvert.Id,
                Advert = newAdvert,
                ImagePath = imagePath,
                ImageSize=imageSize

            };
            _context.AdvertImageEntities.Add(advImage);
            await _context.SaveChangesAsync();

            var selectedCategory = model.CategoryId;
            CategoryAdvertEntity categoryAdvert = new()
            {
                CategoryId = selectedCategory,
                AdvertId = newAdvert.Id,

            };
            _context.CategoryAdvertEntities.Add(categoryAdvert);
            await _context.SaveChangesAsync();

            return View("Add", model);

        }
    }
}

