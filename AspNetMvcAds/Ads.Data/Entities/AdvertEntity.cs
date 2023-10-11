using Ads.Data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public int AdvertClickCount { get; set; }

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }= null!;

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
