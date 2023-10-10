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
		.RuleFor(p => p.CreatedAt, f => f.Date.Past(2));
		

			var pages = pageFaker.Generate(10);
			dbcontext.PageEntities.AddRange(pages);
			dbcontext.SaveChanges();


			var advertFaker = new Faker<AdvertEntity>()
			.RuleFor(a => a.Title, f => f.Lorem.Sentence(5))
			.RuleFor(a => a.Description, f => f.Lorem.Paragraphs())
			.RuleFor(a => a.AdvertClickCount, f => f.Random.Number(0, 1000))
			.RuleFor(a => a.UserId, f => f.Random.Number(1, 100)) // Örnek olarak 1-100 arasında rastgele bir değer.
			.RuleFor(a => a.CreatedAt, f => f.Date.Past(2));
			
			var advert = advertFaker.Generate(50);
			dbcontext.AdvertEntities.AddRange(advert);
			dbcontext.SaveChanges();

			var advertImagefaker = new Faker<AdvertImageEntity>()

				.RuleFor(p => p.ImagePath, f => f.Image.PicsumUrl())
				.RuleFor(p => p.CreatedAt, f => f.Date.PastOffset())
				.RuleFor(p => p.CoverImageInt, f => f.Random.Number(1, 1000))
				.RuleFor(p => p.AdvertId, f => f.Random.Number(1, 50))
				.RuleFor(p => p.UpdatedAt, f => f.Date.PastOffset());
			var advertImage=advertImagefaker.Generate(50);
			dbcontext.AdvertImageEntities.AddRange(advertImage);
			dbcontext.SaveChanges();




			//if (!dbcontext.AdvertImageEntities.Any())
			//{
			//	var image1 = new AdvertImageEntity
			//	{
			//		ImagePath = "products-1.jpg",
			//		AdvertId = 1,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now,
			//	};

			//	var image2 = new AdvertImageEntity
			//	{
			//		ImagePath = "products-3.jpg",
			//		AdvertId = 2,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now,
			//	};

			//	var image3 = new AdvertImageEntity
			//	{
			//		ImagePath = "products-4.jpg",
			//		AdvertId = 2,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now,
			//	};

			//	dbcontext.AdvertImageEntities.AddRange(image1, image2);
			//	dbcontext.SaveChanges();


			//}

			var categoryFaker = new Faker<CategoryEntity>()
			.RuleFor(c => c.Name, f => f.Commerce.Department())
			.RuleFor(c => c.Description, f => f.Lorem.Sentence(10));
			var category = categoryFaker.Generate(20);
			dbcontext.CategoryEntities.AddRange(category);
			dbcontext.SaveChanges();


			var categoryAdvertFaker = new Faker<CategoryAdvertEntity>()
			.RuleFor(ca => ca.CategoryId, f => f.Random.Int(1, 20))
			.RuleFor(ca => ca.AdvertId, f => f.Random.Int(1, 50));
			var categoryAdvert = categoryAdvertFaker.Generate(10);
			dbcontext.CategoryAdvertEntities.AddRange(categoryAdvert);
			dbcontext.SaveChanges();

			var ComentFaker = new Faker<AdvertCommentEntity>()
			   .RuleFor(ac => ac.Comment, f => f.Lorem.Paragraph())
			   .RuleFor(ac => ac.IsActive, f => f.Random.Bool())
			   .RuleFor(ac => ac.AdvertId, f => f.Random.Int(1, 50))
			   .RuleFor(ac => ac.UserId, f => f.Random.Int(1, 100))
			   .RuleFor(ac => ac.CreatedAt, f => f.Date.Past(1));
			  
			var coment = ComentFaker.Generate(100);
			dbcontext.AdvertCommentEntities.AddRange(coment);
			dbcontext.SaveChanges();


			// AdvertCommentEntity Seedleri
			//if (!dbcontext.AdvertCommentEntities.Any())
			//{
			//	var user = dbcontext.UserEntities.First();

			//	var advertComment1 = new AdvertCommentEntity
			//	{
			//		Comment = "Bu bir yorumdur.",
			//		IsActive = true,
			//		AdvertId = 1,
			//		UserId = 1,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now
			//	};

			//	var advertComment2 = new AdvertCommentEntity
			//	{
			//		Comment = "Başka bir yorum.",
			//		IsActive = false,
			//		AdvertId = 2,
			//		UserId = 2,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now
			//	};

			//	var advertComment3 = new AdvertCommentEntity
			//	{
			//		Comment = "Bu bir yorumdur.",
			//		IsActive = true,
			//		AdvertId = 3,
			//		UserId = 3,
			//		CreatedAt = DateTimeOffset.Now,
			//		UpdatedAt = DateTimeOffset.Now
			//	};

			//	dbcontext.AdvertCommentEntities.AddRange(advertComment1, advertComment2, advertComment3);
			//	dbcontext.SaveChanges();
			//}


			// SettingEntity Seedleri
			if (!dbcontext.SettingEntities.Any())
			{
				var user = dbcontext.UserEntities.First();

				var setting1 = new SettingEntity
				{
					Name = "Setting1",
					Value = "Value1",
					UserId = user.Id,
					CreatedAt = DateTimeOffset.Now,
					UpdatedAt = DateTimeOffset.Now,
				};


				var setting2 = new SettingEntity
				{
					Name = "Setting2",
					Value = "Value2",
					UserId = user.Id,
					CreatedAt = DateTimeOffset.Now,
					UpdatedAt = DateTimeOffset.Now,
				};

				var setting3 = new SettingEntity
				{
					Name = "Setting3",
					Value = "Value3",
					UserId = user.Id,
					CreatedAt = DateTimeOffset.Now,
					UpdatedAt = DateTimeOffset.Now,
				};


				dbcontext.SettingEntities.AddRange(setting1, setting2, setting3);
				dbcontext.SaveChanges();
			}



		}
	}
}

