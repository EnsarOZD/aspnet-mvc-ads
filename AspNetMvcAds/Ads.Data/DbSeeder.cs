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


			var pageTitles = new string[]
			{
				"About Us",
				"Contact Us",
				"User Profile",
				"404 Pages",
				"Package",
				"Single Page",
				"Store Single",
				"Single Post",
				"Blog"

			};

			var pageFaker = new Faker<PageEntity>()
            .RuleFor(p => p.Title, f => f.PickRandom(pageTitles))
            .RuleFor(p => p.Content, f => f.Lorem.Paragraphs())
			.RuleFor(p => p.IsActive, f => f.Random.Bool())
			.RuleFor(p => p.CreatedAt, f => f.Date.Past(2));


			var pages = pageFaker.Generate(10);
			dbcontext.PageEntities.AddRange(pages);
			dbcontext.SaveChanges();

			var categoryIds = dbcontext.CategoryEntities.Select(c => c.Id).ToList();

			var advertFaker = new Faker<AdvertEntity>()
			.RuleFor(a => a.Title, f => f.Lorem.Sentence(5))
			.RuleFor(a => a.Description, f => f.Lorem.Sentence(10))
			.RuleFor(a => a.AdvertClickCount, f => f.Random.Number(0, 1000))
			.RuleFor(a => a.UserId, f => f.Random.Number(1, 100))
			.RuleFor(a => a.CreatedAt, f => f.Date.Past(2));


			var advert = advertFaker.Generate(50);
			dbcontext.AdvertEntities.AddRange(advert);
			dbcontext.SaveChanges();

			var advertImagefaker = new Faker<AdvertImageEntity>()


			.RuleFor(p => p.ImagePath, f => f.Image.PicsumUrl())
			.RuleFor(p => p.CreatedAt, f => f.Date.PastOffset())
			.RuleFor(p => p.CoverImageInt, f => f.Random.Number(1, 1000))
			.RuleFor(p => p.AdvertId, f => f.Random.Number(1, 50))
			.RuleFor(p => p.UpdatedAt, f => f.Date.PastOffset())
			.RuleFor(p => p.StarCount, f => f.Random.Number(1, 5));
			var advertImage = advertImagefaker.Generate(50);
			dbcontext.AdvertImageEntities.AddRange(advertImage);
			dbcontext.SaveChanges();


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


			var settingFaker = new Faker<SettingEntity>()
			.RuleFor(s => s.Name, f => f.System.FileName()) 
			.RuleFor(s => s.Value, f => f.Random.String2(10, 400)) 
			.RuleFor(s => s.UserId, f => f.Random.Int(1, 100)); 
			var setting = settingFaker.Generate(10);
			dbcontext.SettingEntities.AddRange(setting);
			dbcontext.SaveChanges();


		}
	}
}

