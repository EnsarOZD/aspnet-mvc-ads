using Ads.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<AdvertCommentEntity> AdvertCommentEntities { get; set; }
        public DbSet<AdTypeEntity> AddTypeEntities { get; set; }
        public DbSet<AdvertEntity> AdvertEntities { get; set; }
        public DbSet<AdvertImageEntity> AdvertImageEntities { get; set; }
        public DbSet<CategoryAdvertEntity> CategoryAdvertEntities { get; set; }
        public DbSet<CategoryEntity> CategoryEntities { get; set; }
        public DbSet<PageEntity> PageEntities { get; set; }
        public DbSet<SettingEntity> SettingEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }
        public DbSet<UserImageEntity> UserImageEntities { get; set; }
        public DbSet<TokenUsage> TokenUsages { get; set; }

        public DbSet<ContactUsMessagesEntity> ContactUsMessage { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    }
}
