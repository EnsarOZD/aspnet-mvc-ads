using Ads.Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data.Entities
{
    public class AdvertImageEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string ImagePath { get; set; } = string.Empty;
        public int AdvertId { get; set; }
        public int CoverImageInt { get; set; }
		public long? ImageSize { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

        // Navigation Properties
        [ForeignKey(nameof(AdvertId))]
        public AdvertEntity Advert { get; set; } = null!;
    }
}
