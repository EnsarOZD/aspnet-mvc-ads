using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userName = User.Identity.Name;
            var user = _context.UserEntities.FirstOrDefault(u => u.Name == userName);

            if (user != null)
            {
                var viewmodel = new ProductViewModel
                {
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
        public IActionResult Edit() 
        {
            return View();
        }
    }
}
