using Ads.Data;
using Ads.Web.Mvc.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace Ads.Web.Mvc.Areas.Admin.ViewComponents
{
    public class AdminNavBarViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public AdminNavBarViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public ViewViewComponentResult Invoke()
        {
            if (User.Identity.IsAuthenticated)
            {
                var claimsPrincipal = User as ClaimsPrincipal;
                var userId = int.TryParse(claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid), out int result) ? result.ToString() : null;

                var user = _db.UserEntities.FirstOrDefault(x => x.Id.ToString() == userId);
                var userImage = _db.UserImageEntities.FirstOrDefault(x => x.UserId == user.Id);

                var navbarList = new NavbarListViewModel
                {
                    Id = user.Id,
                    Categories = _db.CategoryEntities.ToList(),
                    Pages = _db.PageEntities.ToList(),
                    Name = user.Name,
                    UserImagePath = userImage?.ImagePath,
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
