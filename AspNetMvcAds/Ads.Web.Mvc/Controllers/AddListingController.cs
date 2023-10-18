using Ads.Data;
using Ads.Data.Entities;
using Ads.Services.Services;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            //ViewData["Categories"] = new SelectList(categories, "Id", "Name");
            ViewData["Categories"] = new SelectList(categories, "Name", "Name");
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
            ViewData["Categories"] = new SelectList(categories, "Id", "Name");


            AddListingEntity add = new()
            {
                ContactEmail = model.ContactEmail,
                AdType = model.AdType,
                AcceptTermsAndConditions = model.AcceptTermsAndConditions,
                AdFeature = model.AddFeature,
                //AdvertImages = model.AdvertImages,
                //CategoryId = model.CategoryId,
                CategoryName=model.CategoryName,
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
                User = model.User,
                //Advert = model.Advert,
            };
            _context.AddListingEntities.Add(add);
            await _context.SaveChangesAsync();




            return View("Add", model);

        }
    }
}

