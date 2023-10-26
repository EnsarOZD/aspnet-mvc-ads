using Ads.Data.Entities;
using Ads.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ads.Services.Models;

namespace Ads.Services.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public SearchViewModel SearchAdvertsByTitle(string searchContent)
        {
            var query = from s in _context.AdvertEntities
                        select s;
            var images = _context.AdvertImageEntities.ToList();

            if (!string.IsNullOrEmpty(searchContent))
            {
                query = query.Where(p => p.Title.Contains(searchContent));
            }

            var viewModel = new SearchViewModel
            {
                AdvertEntities = query.ToList(),
                AdvertImageEntities = images
            };

            return viewModel;
        }

    }
}
