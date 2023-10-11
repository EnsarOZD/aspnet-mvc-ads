using Ads.Data.Entities;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data
{
    public class DbSeeder
    {
        public static void Seed(AppDbContext dbcontext)
        {
            var fakeuser = new Faker<UserEntity>()
           .RuleFor(u => u.Email, f => f.Internet.Email())
           .RuleFor(u => u.Password, f => f.Internet.Password())
           .RuleFor(u => u.Name, f => f.Name.FirstName())
           .RuleFor(u => u.Address, f => f.Address.FullAddress())
           .RuleFor(u => u.Phone, f =>
           {
               var phone = f.Phone.PhoneNumber();
               return phone.Length >= 20 ? phone.Substring(0, 20) : phone;
           });
            var users = fakeuser.Generate(100);
            dbcontext.UserEntities.AddRange(users);
            dbcontext.SaveChanges();


            var pageFaker = new Faker<PageEntity>()
        .RuleFor(p => p.Title, f =>
        {
            var sentence = f.Lorem.Sentence(5);
            return sentence.Substring(0, Math.Min(200, sentence.Length));
        })
        .RuleFor(p => p.Content, f => f.Lorem.Paragraphs())
        .RuleFor(p => p.IsActive, f => f.Random.Bool())
        .RuleFor(p => p.CreatedAt, f => f.Date.Past(2))
        .RuleFor(p => p.UpdatedAt, (f, p) => f.Date.Between(p.CreatedAt, DateTime.Now));

            var pages = pageFaker.Generate(10);
            dbcontext.PageEntities.AddRange(pages);
            dbcontext.SaveChanges();

            //var pages = new List<PageEntity>
            //         {
            //              new PageEntity
            //              {
            //                  Title = "About Us",
            //                  Content = "This is the About Us page content.",
            //                  IsActive = true,
            //                  CreatedAt = DateTime.Now,
            //                  UpdatedAt = DateTime.Now,
            //              },
            //              new PageEntity
            //              {
            //                 Title = "Contact Us",
            //                 Content = "You can reach us at contact@example.com.",
            //                 IsActive = true,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //              },
            //              new PageEntity
            //              {
            //                  Title = "Privacy Policy",
            //                  Content = "This is the Privacy Policy page content.",
            //                  IsActive = true,
            //                  CreatedAt = DateTime.Now,
            //                  UpdatedAt = DateTime.Now,
            //              }
            //         };
            //         dbcontext.PageEntities.AddRange(pages);
            //         dbcontext.SaveChanges();

            var advertFaker = new Faker<AdvertEntity>()
            .RuleFor(a => a.Title, f => f.Lorem.Sentence(5))
            .RuleFor(a => a.Description, f => f.Lorem.Paragraphs())
            .RuleFor(a => a.AdvertClickCount, f => f.Random.Number(0, 1000))
            .RuleFor(a => a.UserId, f => f.Random.Number(1, 100)) // Örnek olarak 1-100 arasında rastgele bir değer.
            .RuleFor(a => a.CreatedAt, f => f.Date.Past(2))
            .RuleFor(a => a.UpdatedAt, (f, a) => f.Date.Between(a.CreatedAt, DateTime.Now));
            var advert = advertFaker.Generate(50);
            dbcontext.AdvertEntities.AddRange(advert);
            dbcontext.SaveChanges();


            //if (!dbcontext.AdvertEntities.Any())
            //         {
            //             var user = dbcontext.UserEntities.First();
            //             var advert1 = new AdvertEntity
            //             {

            //                 Title = "Monitor",
            //                 Description = "Monitor",
            //                 UserId = user.Id,
            //                 AdvertClickCount = 1,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert2 = new AdvertEntity
            //             {

            //                 Title = "11inch Macbook Air",
            //                 Description = "Macbook",
            //                 UserId = user.Id,
            //                 AdvertClickCount=100,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert3 = new AdvertEntity
            //             {

            //                 Title = "Study Table Combo",
            //                 Description = "Table",
            //                 UserId = user.Id,
            //		AdvertClickCount = 3,
            //		CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert4 = new AdvertEntity
            //             {

            //                 Title = "advert4",
            //                 Description = "advert4",
            //                 UserId = user.Id,
            //                 AdvertClickCount = 75,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert5 = new AdvertEntity
            //             {

            //                 Title = "advert5",
            //                 Description = "advert5",
            //                 UserId = user.Id,
            //                 AdvertClickCount = 3,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert6 = new AdvertEntity
            //             {

            //                 Title = "advert6",
            //                 Description = "advert6",
            //                 UserId = user.Id,
            //                 AdvertClickCount = 3,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             var advert7 = new AdvertEntity
            //             {

            //                 Title = "advert7",
            //                 Description = "advert7",
            //                 UserId = user.Id,
            //                 AdvertClickCount = 3,
            //                 CreatedAt = DateTime.Now,
            //                 UpdatedAt = DateTime.Now,
            //             };

            //             dbcontext.AdvertEntities.AddRange(advert1, advert2, advert3, advert4, advert5, advert6, advert7);
            //             dbcontext.SaveChanges();

            //}



            if (!dbcontext.AdvertImageEntities.Any())
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

                dbcontext.AdvertImageEntities.AddRange(image1, image2);
                dbcontext.SaveChanges();

                if (!dbcontext.CategoryEntities.Any())
                {
                    var user = dbcontext.UserEntities.First();

                    var Category1 = new CategoryEntity
                    {
                        Name = "Job",
                        Description = "Looking for job",
                    };
                    var Category2 = new CategoryEntity
                    {
                        Name = "Sale",
                        Description = "Whole Sale",
                    };
                    var Category3 = new CategoryEntity
                    {
                        Name = "Second Hand",
                        Description = "Second hand products"
                    };
                    var Category4 = new CategoryEntity
                    {
                        Name = "Hotel",
                        Description = "Rezervation For Hotel"
                    };
                    var Category5 = new CategoryEntity
                    {
                        Name = "Elektronic",
                        Description = "Electronic products"
                    };
                    var Category6 = new CategoryEntity
                    {
                        Name = "Fitness",
                        Description = "For healty"
                    };
                    var Category7 = new CategoryEntity
                    {
                        Name = "Cars",
                        Description = "Cars"
                    };
                    var Category8 = new CategoryEntity
                    {
                        Name = "Restourants",
                        Description = "Eating for Food"
                    };
                    var Category9 = new CategoryEntity
                    {
                        Name = "Cafe",
                        Description = "For have a good time"
                    };
                    dbcontext.CategoryEntities.AddRange(Category1, Category2, Category3, Category4, Category5, Category6, Category7, Category8, Category9);
                    dbcontext.SaveChanges();


                }
                if (!dbcontext.CategoryAdvertEntities.Any())
                {
                    var user = dbcontext.UserEntities.First();

                    var CategoryAd1 = new CategoryAdvertEntity
                    {
                        CategoryId = 1,
                        AdvertId = 1
                    };
                    var CategoryAd2 = new CategoryAdvertEntity
                    {
                        CategoryId = 2,
                        AdvertId = 2,
                    };
                    var CategoryAd3 = new CategoryAdvertEntity
                    {
                        CategoryId = 3,
                        AdvertId = 3
                    };
                    var CategoryAd4 = new CategoryAdvertEntity
                    {
                        CategoryId = 4,
                        AdvertId = 4
                    };
                    var CategoryAd5 = new CategoryAdvertEntity
                    {
                        CategoryId = 5,
                        AdvertId = 5
                    };
                    var CategoryAd6 = new CategoryAdvertEntity
                    {
                        CategoryId = 3,
                        AdvertId = 7
                    };
                    var CategoryAd7 = new CategoryAdvertEntity
                    {
                        CategoryId = 7,
                        AdvertId = 7
                    };
                    var CategoryAd8 = new CategoryAdvertEntity
                    {
                        CategoryId = 8,
                        AdvertId = 8
                    };
                    var CategoryAd9 = new CategoryAdvertEntity
                    {
                        CategoryId = 9,
                        AdvertId = 9
                    };
                    dbcontext.CategoryAdvertEntities.AddRange(CategoryAd1, CategoryAd2, CategoryAd3, CategoryAd4, CategoryAd5, CategoryAd6, CategoryAd7, CategoryAd8, CategoryAd9);
                    dbcontext.SaveChanges();


                }
                // AdvertCommentEntity Seedleri
                if (!dbcontext.AdvertCommentEntities.Any())
                {
                    var user = dbcontext.UserEntities.First();

                    var advertComment1 = new AdvertCommentEntity
                    {
                        Comment = "Bu bir yorumdur.",
                        IsActive = true,
                        AdvertId = 1,
                        UserId = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    var advertComment2 = new AdvertCommentEntity
                    {
                        Comment = "Başka bir yorum.",
                        IsActive = false,
                        AdvertId = 2,
                        UserId = 2,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    var advertComment3 = new AdvertCommentEntity
                    {
                        Comment = "Bu bir yorumdur.",
                        IsActive = true,
                        AdvertId = 3,
                        UserId = 3,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    dbcontext.AdvertCommentEntities.AddRange(advertComment1, advertComment2, advertComment3);
                    dbcontext.SaveChanges();
                }


                // SettingEntity Seedleri
                if (!dbcontext.SettingEntities.Any())
                {
                    var user = dbcontext.UserEntities.First();

                    var setting1 = new SettingEntity
                    {
                        Name = "Setting1",
                        Value = "Value1",
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };


                    var setting2 = new SettingEntity
                    {
                        Name = "Setting2",
                        Value = "Value2",
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };

                    var setting3 = new SettingEntity
                    {
                        Name = "Setting3",
                        Value = "Value3",
                        UserId = user.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };


                    dbcontext.SettingEntities.AddRange(setting1, setting2, setting3);
                    dbcontext.SaveChanges();
                }



            }
        }
    }
}
