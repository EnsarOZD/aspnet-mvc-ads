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
                var userIdClaim = claimsPrincipal?.FindFirstValue(ClaimTypes.PrimarySid);
                if (int.TryParse(userIdClaim, out int userId))
                {
                    var user = _db.UserEntities.FirstOrDefault(x => x.Id == userId);
                    if (user != null) // user nesnesi kontrol ediliyor
                    {
                        var userImage = _db.UserImageEntities.FirstOrDefault(x => x.UserId == user.Id);
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
                    // user null ise uygun bir işlem yapın veya hata döndürün
                }
            }

            // Eğer kullanıcı doğrulanmamışsa veya yukarıdaki if bloğu herhangi bir nedenle geçilmemişse
            var navbarListFallback = new NavbarListViewModel
            {
                Categories = _db.CategoryEntities.ToList(),
                Pages = _db.PageEntities.ToList(),
            };
            return View(navbarListFallback);
        }
    }
}
