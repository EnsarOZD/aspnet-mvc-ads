using Ads.Data;
using Ads.Web.Mvc.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace Ads.Web.Mvc.ViewComponents
{
    public class NavbarViewComponent:ViewComponent
    {
        private readonly AppDbContext _db;

        public NavbarViewComponent(AppDbContext db)
        {
            _db = db;
        }

        public ViewViewComponentResult Invoke()
        {
            var navbarList = new NavbarListViewModel
            {
                Categories = _db.CategoryEntities.ToList(),
                Pages=_db.PageEntities.ToList()
             
            };

            return View(navbarList);
        }
    }
}
