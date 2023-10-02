﻿using Ads.Data.Entities;
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
            var users = new List<UserEntity>
            {
                new UserEntity
                {
                    Email = "user1@example.com",
                    Password = "password1",
                    Name = "User 1",
                    Address = "123 Main Street",
                    Phone = "555-123-4567",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },
                new UserEntity
                {
                    Email = "user2@example.com",
                    Password = "password2",
                    Name = "User 2",
                    Address = "456 Elm Street",
                    Phone = "555-987-6543",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,

                },
                new UserEntity
                {
                    Email = "user3@example.com",
                    Password = "password3",
                    Name = "User 3",
                    Address = "789 Oak Street",
                    Phone = "555-555-5555",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                },

            };
            dbcontext.UserEntities.AddRange(users);
            dbcontext.SaveChanges();

            var pages = new List<PageEntity>
            {
                 new PageEntity
                 {
                     Title = "About Us",
                     Content = "This is the About Us page content.",
                     IsActive = true,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now,
                 },
                 new PageEntity
                 {
                    Title = "Contact Us",
                    Content = "You can reach us at contact@example.com.",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                 },
                 new PageEntity
                 {
                     Title = "Privacy Policy",
                     Content = "This is the Privacy Policy page content.",
                     IsActive = true,
                     CreatedAt = DateTime.Now,
                     UpdatedAt = DateTime.Now,
                 }
            };
            dbcontext.PageEntities.AddRange(pages);
            dbcontext.SaveChanges();

            if (!dbcontext.AdvertEntities.Any())
            {
                var user = dbcontext.UserEntities.First();
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

                dbcontext.AdvertEntities.AddRange(advert1, advert2, advert3);
                dbcontext.SaveChanges();

            }

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
                        Name = "job",
                        Description = "Looking for job",
                    };
                    var Category2 = new CategoryEntity
                    {
                        Name = "Sale",
                        Description = "Whole Sale",
                    };
                    var Catagory3 = new CategoryEntity
                    {
                        Name = "Second Hand",
                        Description = "Second hand products"
                    };
                    dbcontext.CategoryEntities.AddRange(Category1, Category2, Catagory3);
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
                    var CatagoryAd3 = new CategoryAdvertEntity
                    {
                        CategoryId = 3,
                        AdvertId = 3
                    };
                    dbcontext.CategoryAdvertEntities.AddRange(CategoryAd1, CategoryAd2, CatagoryAd3);
                    dbcontext.SaveChanges();


                }


            }
        }
    }
}