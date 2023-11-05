using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Controllers;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var userName = User.Identity.Name;
            var user = _context.UserEntities.FirstOrDefault(u => u.Name == userName);

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
                Email = user.Email,
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

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Your name has changed successfully";
                
            }
            //return RedirectToAction("Edit","User");
            return RedirectToAction("Index", "User");
        }

    }
}

