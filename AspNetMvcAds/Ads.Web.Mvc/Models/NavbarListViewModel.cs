using Ads.Data.Entities;

namespace Ads.Web.Mvc.Models
{
    public class NavbarListViewModel
    {
        public List<CategoryEntity> Categories { get; set; }
        public List<PageEntity> Pages { get; set; }
        public string Name { get; set; }
        public string UserImagePath { get; set; }
        public int Id { get; set; }

    }
}