﻿using Ads.Data;
using Ads.Data.Entities;
using Ads.Services.Services;
using Ads.Services.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Controllers;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AdvertViewModel = Ads.Web.Mvc.Models.AdvertViewModel;

namespace Ads.Web.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        public UserController(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id.ToString() == userId);

            if (user != null)
            {
                var userImage = await _context.UserImageEntities.FirstOrDefaultAsync(x => x.UserId == user.Id);
                var userAdverts = await _context.AdvertEntities
     .Where(ai => ai.UserId.ToString() == userId)
     .ToListAsync();

                var imagePath = userImage != null ? userImage.ImagePath : "default_image_path.jpg";
                var viewmodel = new ProductViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    ImagePath = imagePath,


                    AdvertEntities = _context.AdvertEntities
                        .Where(a => a.UserId == user.Id)
                        .ToList(),
                    AdvertImageEntities = _context.AdvertImageEntities
                        .Where(ai => ai.Advert.User.Id == user.Id)
                        .ToList(),
                    CategoryAdvertEntities = _context.CategoryAdvertEntities
                        .Where(ca => ca.Advert.User.Id == user.Id)
                        .ToList(),
                    CategoryEntities = _context.CategoryEntities.ToList(),

                };

                return View(viewmodel);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AdvertDetail(int id)
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var userAdverts = await _context.AdvertEntities
    .Where(ai => ai.UserId.ToString() == userId)
    .ToListAsync();
            var advert = _context.AdvertEntities.Find(id);
            var advertImage = _context.AdvertImageEntities.Find(id);
            var categoryAdvert = await _context.CategoryAdvertEntities
                    .Include(ca => ca.Category)
                    .FirstOrDefaultAsync(ca => ca.AdvertId == id);
            var categoryName = await _context.CategoryEntities.FindAsync(categoryAdvert.Id);
            if (advert == null)
            {
                return NotFound();
            }

            var viewModel = new AdvertViewModel
            {
                Id = advert.Id,
                Title = advert.Title,
                Description = advert.Description,
                CreatedAt = advert.CreatedAt,
                ImagePath = advertImage.ImagePath,
                AdFeature = advert.AdFeature,
                PaymentMethod = advert.PaymentMethod,
                Price = advert.Price,
                CategoryName = categoryAdvert.Category.Name

            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit([FromRoute] int id)
        {
            var user = _context.UserEntities.Find(id);
            var userImage = _context.UserImageEntities.FirstOrDefault(x => x.UserId == id);


            if (user == null)
            {
                return RedirectToAction("Index");
            }

            var userImagePath = userImage != null ? userImage.ImagePath : "default_image_path.jpg"; // Varsayılan bir resim yolu ekleyin

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                NewEmail = user.Email,
                Address = user.Address,
                Phone = user.Phone,
                UserImagePath = userImagePath
            };

            return View(userViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> EditName([FromRoute] int id, ProductViewModel userName)
        {
            var user = await _context.UserEntities.FindAsync(id);

            if (user != null)
            {
                user.Name = userName.Name;
                user.Id = userName.Id;
                user.Address = userName.Address;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your personnel information has changed successfully";

            }

            return RedirectToAction("Edit", "User", new { id = id });

        }
        [HttpPost]
        public async Task<IActionResult> EditPassword([FromRoute] int id, ProductViewModel userName)
        {
            var user = await _context.UserEntities.FindAsync(id);
            var userOldPassword = user.Password;


            if (user != null && userName.Password == userName.PasswordVerify && userOldPassword == userName.CurrentPassword)
            {
                user.Password = userName.Password;
                user.Id = userName.Id;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your password has changed successfully";

                return RedirectToAction("Edit", "User", new { id = id });
            }
            else
            {
                TempData["PasswordErrorMessage"] = "Your passwords doesnt match or wrong current password";
            }
            return RedirectToAction("Edit", "User", new { id = id });

        }

        [HttpPost]
        public async Task<IActionResult> EditEmail([FromRoute] int id, UserViewModel userName)
        {
            var user = await _context.UserEntities.FindAsync(id);

            if (user != null)
            {
                if (userName.NewEmail == userName.VerifyNewEmail)
                {
                    user.Email = userName.NewEmail;
                    user.Id = userName.Id;
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Your E-mail has changed successfully";
                    return RedirectToAction("Edit", "User", new { id = id });
                }
                else
                {
                    TempData["ErrorMessage"] = "Emails should match!";
                }
            }
            return RedirectToAction("Edit", "User", new { id = id });
        }
        public async Task<IActionResult> AddImage([FromRoute] int id, UserViewModel model, [FromForm] IFormFile formFile)
        {
            if (formFile != null)
            {
                if (formFile.Length > 2 * 1024 * 1024)
                {
                    ModelState.AddModelError("File", "Dosya boyutu 2 MB'dan büyük olamaz.");
                    ViewBag.Error = "Dosya boyutu 2 MB'dan büyük olamaz.";
                    return RedirectToAction("Edit", "User", new { id = id });
                }

                if (Path.GetExtension(formFile.FileName).ToLower() != ".jpg")
                {
                    ModelState.AddModelError("File", "Sadece .jpg uzantılı dosyaları yükleyebilirsiniz.");
                    ViewBag.Error = "Sadece.jpg uzantılı dosyaları yükleyebilirsiniz.";
                    return RedirectToAction("Edit", "User", new { id = id });
                }

                await _fileService.UploadFileAsync(formFile);
                string imageName = formFile.FileName;
                long imageSize = formFile.Length;
                var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
                var user = _context.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);
                UserImageEntity userImage = new()
                {
                    CreatedAt = DateTime.Now,
                    Id = id,
                    ImagePath = $"/uploads/{imageName}",
                    UserId = user.Id
                };
                _context.UserImageEntities.Add(userImage);
                await _context.SaveChangesAsync();
                TempData["UploadMessage"] = "Ad Listed Successfully";
            }
            return RedirectToAction("Edit", "User", new { id = id });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await DeleteCommentAsync(id);
            TempData["SuccessMessage"] = "Comment deleted succesfully";
            return RedirectToAction("Index");
        }
        public async Task DeleteCommentAsync(int id)
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var userAdverts = await _context.AdvertEntities
    .Where(ai => ai.UserId.ToString() == userId)
    .ToListAsync();
            var advert = _context.AdvertEntities.Find(id);
            if (advert != null)
            {
                _context.AdvertEntities.Remove(advert);
                await _context.SaveChangesAsync();
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditAdvert(int id)
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var userAdverts = await _context.AdvertEntities
    .Where(ai => ai.UserId.ToString() == userId)
    .ToListAsync();
            var advert = _context.AdvertEntities.Find(id);
            var categoryAdvertId = await _context.CategoryAdvertEntities.FirstOrDefaultAsync(ca => ca.AdvertId == id);
            var categoryName = await _context.CategoryEntities.FindAsync(categoryAdvertId.Id);
            var categories = _context.CategoryEntities.ToList();
            ViewData["Categories"] = new SelectList(categories, nameof(CategoryEntity.Id), nameof(CategoryEntity.Name));



            var viewModel = new AdvertViewModel
            {
                Id = advert.Id,
                Title = advert.Title,
                Description = advert.Description,
                CreatedAt = advert.CreatedAt,
                AdFeature = advert.AdFeature,
                Price = advert.Price,
                CategoryName = categoryName.Name,
                PaymentMethod = advert.PaymentMethod,
                CategoryId = categoryAdvertId?.CategoryId


            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditAdvert(int id, AdvertViewModel advertViewModel, [FromForm] IFormFile formFile)
        {
            var advert = await _context.AdvertEntities.FindAsync(id);


            if (advert != null)
            {
                advert.Id = advertViewModel.Id;
                advert.Title = advertViewModel.Title;
                advert.Description = advertViewModel.Description;
                advert.Price = advertViewModel.Price;
                advert.AdFeature = advertViewModel.AdFeature;
                advert.PaymentMethod = advertViewModel.PaymentMethod;

                var categoryAdvertId = await _context.CategoryAdvertEntities.FirstOrDefaultAsync(ca => ca.AdvertId == id);
                if (categoryAdvertId != null)
                {
                    categoryAdvertId.CategoryId = advertViewModel.CategoryId ?? 0;

                }
                ViewBag.SuccessMessage = "Your Advert Edited Successfully";
            }
            await _context.SaveChangesAsync();
            var viewmodel = new AdvertViewModel
            {
                Id = id,
                Title = advert.Title,
                Description = advert.Description,
                Price = advert.Price,
                AdFeature = advert.AdFeature,
                PaymentMethod = advert.PaymentMethod,
            };
            return View(viewmodel);

        }
    }
}



