using Ads.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Models
{
    public class SearchViewModel
    {
        public List<AdvertEntity> AdvertEntities { get; set; }
        public List<AdvertImageEntity> AdvertImageEntities { get; set; }
       
    }
}
