using Ads.Data;

using Ads.Services.Services;

using Ads.Data.Entities;

using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public IActionResult Index(string searchContent)
        {
            var searchResult = _searchService.SearchAdvertsByTitle(searchContent);

            return View(searchResult);
        }
    }
}
