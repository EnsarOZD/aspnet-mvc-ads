﻿using Ads.Data;

using Ads.Services.Services;

using Ads.Data.Entities;

using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        public SearchController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(string searchContent, int? categoryId, string location)
        {
            IQueryable<AdvertEntity> advertTitle = _context.AdvertEntities;

            IQueryable<CategoryEntity> advertCategory = _context.CategoryEntities;


            if (!string.IsNullOrEmpty(searchContent))
            {
                advertTitle = advertTitle.Where(p => p.Title.Contains(searchContent));

            }

            if (categoryId is not null)
            {
                var advertIds = _context.CategoryAdvertEntities
                    .Where(x => x.CategoryId == categoryId)
                    .Select(x => x.AdvertId)
                    .ToList();

                advertTitle = advertTitle.Where(x => advertIds.Contains(x.Id));
            }


            var titles = advertTitle.ToList();
            var categories = advertCategory.ToList();
            var viewModel = new SearchViewModel
            {
                AdvertEntities = titles,
                CategoryEntities = categories,
                AdvertImageEntities = _context.AdvertImageEntities.ToList(),
                CategoryAdvertEntities = _context.CategoryAdvertEntities.ToList()

            };
            return View(viewModel);
        }
    }
}
