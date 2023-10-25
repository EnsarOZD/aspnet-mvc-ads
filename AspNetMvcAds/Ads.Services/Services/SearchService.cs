using Ads.Data.Entities;
using Ads.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Services
{
    public class SearchService : ISearchService
    {
        private readonly AppDbContext _context;

        public SearchService(AppDbContext context)
        {
            _context = context;
        }

        public List<AdvertEntity> SearchAdvertsByTitle(string searchContent)
        {
            var query = from s in _context.AdvertEntities
                        select s;

            if (!string.IsNullOrEmpty(searchContent))
            {
                query = query.Where(p => p.Title.Contains(searchContent));
            }

            return query.ToList();
        }
    }
}
