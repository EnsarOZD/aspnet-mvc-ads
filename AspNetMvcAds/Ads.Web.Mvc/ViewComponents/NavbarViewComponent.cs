using Ads.Data;
using Ads.Web.Mvc.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ads.Web.Mvc.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;

        public NavbarViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public ViewViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var userId = int.TryParse(claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;
                var userImage = _db.UserImageEntities.FirstOrDefault();

                var user = _db.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);

                var navbarList = new NavbarListViewModel
                {
                    Id = user.Id,
                    Categories = _db.CategoryEntities.ToList(),
                    Pages = _db.PageEntities.ToList(),
                    Name = user.Name,
                    UserImagePath = userImage?.ImagePath, // Profil resmi null olabilir
                };
                return View(navbarList);
            }
            else
            {
                var navbarList = new NavbarListViewModel
                {
                    Categories = _db.CategoryEntities.ToList(),
                    Pages = _db.PageEntities.ToList(),
                };
                return View(navbarList);
            }
        }
    }
}
