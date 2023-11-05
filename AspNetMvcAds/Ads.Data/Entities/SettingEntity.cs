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
    public class SettingEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(400)]
        public string Value { get; set; } = string.Empty;

        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
