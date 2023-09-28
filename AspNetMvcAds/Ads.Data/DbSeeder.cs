using Ads.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // verileri kontrol edebilmek için mecburen users eklemek zorunda kaldım. Seeding tamamlanınca kaldırırız.
            var user1 = new UserEntity
            {
                Email = "user1@example.com",
                Password = "password1",
                Name = "User 1",
                Address = "Address 1",
                Phone = "1234567890",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            context.UserEntities.Add(user1);
            context.SaveChanges();
            

            if (!context.AdvertEntities.Any())
            {
                var user = context.UserEntities.First();
                var advert1 = new AdvertEntity
                {

                    Title = "Monitor",
                    Description = "Monitor",
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var advert2 = new AdvertEntity
                {

                    Title = "11inch Macbook Air",
                    Description = "Macbook",
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var advert3 = new AdvertEntity
                {

                    Title = "Study Table Combo",
                    Description = "Table",
                    UserId = user.Id,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                context.AdvertEntities.AddRange(advert1, advert2, advert3);
                context.SaveChanges();

            }

            if (!context.AdvertImageEntities.Any())
            {
                var image1 = new AdvertImageEntity
                {
                    ImagePath = "products-1.jpg",
                    AdvertId = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var image2 = new AdvertImageEntity
                {
                    ImagePath = "products-3.jpg",
                    AdvertId = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                var image3 = new AdvertImageEntity
                {
                    ImagePath = "products-4.jpg",
                    AdvertId = 2,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                context.AdvertImageEntities.AddRange(image1, image2);
                context.SaveChanges();

            }
        }
    }
}
