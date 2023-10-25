using Ads.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Services
{
    public interface ISearchService
    {
        List<AdvertEntity> SearchAdvertsByTitle(string searchContent);
    }
}
