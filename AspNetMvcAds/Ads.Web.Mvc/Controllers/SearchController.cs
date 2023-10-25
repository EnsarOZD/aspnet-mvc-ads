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

        //TODO: HttpPost olmalı mı çünkü viewında form işlemi method:post şeklinde submit ediyor buton ile?
        public IActionResult Index(string searchContent)
        {
            var titles = _searchService.SearchAdvertsByTitle(searchContent);

            var viewModel = new SearchViewModel
            {
                Titles = titles

            var viewModel = new SearchViewModel
            {
                Titles = titles,
                Categories = categories,

            };

            return View(viewModel);
        }
    }
}
