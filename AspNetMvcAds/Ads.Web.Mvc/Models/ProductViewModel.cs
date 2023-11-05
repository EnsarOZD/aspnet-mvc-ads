using Ads.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public List<AdvertImageEntity> AdvertImageEntities { get; set; }
        public List<UserEntity> UserEntities { get; set; }
        public List<AdvertEntity> AdvertEntities { get; set; }
        public List<CategoryEntity> CategoryEntities { get; set; }
        public List<CategoryAdvertEntity> CategoryAdvertEntities { get; set; }
    }
}
