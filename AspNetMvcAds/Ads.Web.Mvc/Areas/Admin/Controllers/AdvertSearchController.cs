﻿using Ads.Services.Services;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    public class AdvertSearchController : Controller
    {
        private readonly ISearchService _searchService;
        public AdvertSearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        [Area("Admin")]
        public IActionResult Index(string searchContent)
        {
            var titles = _searchService.SearchAdvertsByTitle(searchContent);

            var viewModel = new AdvertSearchViewModel
            {
                Titles = titles
            };

            return View(viewModel);
            
        }
    }
}
