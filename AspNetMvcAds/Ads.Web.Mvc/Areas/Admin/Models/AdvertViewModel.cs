using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdvertViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int AdvertClickCount { get; set; }

        public int UserId { get; set; }

    }
}
