﻿using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Controllers;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ads.Web.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;

            var user = _context.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);


            if (user != null)
            {
                var viewmodel = new ProductViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
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
        public IActionResult Edit([FromRoute] int id)
        {
            var user = _context.UserEntities.Find(id);
            if (user == null)
            {

                return RedirectToAction("Index");
            }

            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                NewEmail = user.Email,
                Address = user.Address,
                Phone = user.Phone,
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

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your name has changed successfully";

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
                    return RedirectToAction("Edit", "User");
                }
                else
                {
                    TempData["ErrorMessage"] = "Emails should match!";
                }
            }
            return RedirectToAction("Edit", "User", new { id = id });
        }
    }
}

