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
    public class AdvertCommentEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, Column(TypeName = "text")]
        public string Comment { get; set; } = string.Empty;
        [Required]
        public bool IsActive { get; set; }
        public int AdvertId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(AdvertId))]
        public AdvertEntity Advert { get; set; } = null!;


		public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }

	}
}
