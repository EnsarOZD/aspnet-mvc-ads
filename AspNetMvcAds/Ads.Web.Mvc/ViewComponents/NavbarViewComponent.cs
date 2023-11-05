using Ads.Data;
using Ads.Web.Mvc.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;


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
                var userName = User.Identity.Name;
                var user = _db.UserEntities.FirstOrDefault(u => u.Name == userName);

                var navbarList = new NavbarListViewModel
                {
                    Id = user.Id,
                    Categories = _db.CategoryEntities.ToList(),
                    Pages = _db.PageEntities.ToList(),
                    Name = user.Name,


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
