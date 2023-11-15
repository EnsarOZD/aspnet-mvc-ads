using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Ads.Data.Entities;

namespace Ads.Web.Mvc.Areas.Admin.Models
{
    public class AdvertViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string? ImagePath { get; set; } = string.Empty;
        [Display(Name = "Upload Image")]
        public IFormFile UploadedImage { get; set; }

		public decimal Price { get; set; }

        public int AdvertClickCount { get; set; }

        public int UserId { get; set; }
        public List<AdvertEntity>? AdvertEntities { get; set; }
        public List<AdvertImageEntity>? AdvertImageEntities { get; set; }
        public List<CategoryEntity>? Categories { get; set; }
        public List<CategoryAdvertEntity>? CategoryAdvertEntities { get; set; }
       

    }
}
