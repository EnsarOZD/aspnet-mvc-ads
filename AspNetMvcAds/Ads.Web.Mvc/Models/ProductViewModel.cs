using Ads.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Ads.Web.Mvc.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordVerify { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public List<int> CategoryIds { get; set; } = new List<int>();
        public List<string> CategoryNames { get; set; } = new List<string>();
        public List<AdvertImageEntity> AdvertImageEntities { get; set; }
        public List<UserEntity> UserEntities { get; set; }
        public List<AdvertEntity> AdvertEntities { get; set; }
        public List<CategoryEntity> CategoryEntities { get; set; }
        public List<CategoryAdvertEntity> CategoryAdvertEntities { get; set; }
    }
}
