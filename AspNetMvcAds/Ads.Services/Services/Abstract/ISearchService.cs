using Ads.Data.Entities;
using Ads.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Services.Abstract
{
    public interface ISearchService
    {
        SearchViewModel SearchAdvertsByTitle(string searchContent);
    }
}
