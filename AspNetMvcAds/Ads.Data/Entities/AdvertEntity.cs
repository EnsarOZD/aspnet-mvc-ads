using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ads.Data.Services.Abstract;

namespace Ads.Data.Entities
{
    public class AdvertEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, Column(TypeName = "ntext")]
        public string Description { get; set; } = string.Empty;
        public string AdType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsPriceNegotiable { get; set; }
        public string ContactName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactAddress { get; set; } = string.Empty;
        public string AdFeature { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public bool AcceptTermsAndConditions { get; set; }
        public int StarCount { get; set; }
        public int AdvertClickCount { get; set; }
        public string? ImagePath { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        public ICollection<AdvertImageEntity> AdvertImages;

	}
}
