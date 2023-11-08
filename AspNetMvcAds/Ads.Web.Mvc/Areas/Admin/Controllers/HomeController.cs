using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = int.TryParse(User.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
            var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id.ToString() == userId);
            if (user != null)
            {
                var userImage = await _context.UserImageEntities.FirstOrDefaultAsync(x => x.UserId == user.Id);

                var imagePath = userImage != null ? userImage.ImagePath : "default_image_path.jpg";


                var viewmodel = new AdminUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserImagePath = imagePath,

                };
                return View(viewmodel);
            }
            return View();
        }
    }
}
