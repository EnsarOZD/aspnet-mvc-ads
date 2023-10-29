using Ads.Data;
using Ads.Data.Entities;
using Ads.Services.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Services.Services.Concrete
{
	public class AdvertImageService : IAdvertImageService
	{
		private readonly AppDbContext _context;

		public AdvertImageService(AppDbContext context)
		{
			_context = context;
		}
		public void AddImage(AdvertImageEntity image)
		{
			_context.AdvertImageEntities.Add(image);
			_context.SaveChanges();
		}

		public void DeleteImage(int id)
		{
			var image=_context.AdvertImageEntities.FirstOrDefault(i=>i.Id == id);
			if (image != null)
			{
				_context.AdvertImageEntities.Remove(image);
				_context.SaveChanges();
			}
		}

        public void GetAdvertTitle(int advertId)
        {
            var advert = _context.AdvertEntities.FirstOrDefault(a => a.Id == advertId);
        }

        public IEnumerable<AdvertImageEntity> GetAllImages()
		{
			return _context.AdvertImageEntities.ToList();
		}

		public AdvertImageEntity GetImageById(int id)
		{
			return _context.AdvertImageEntities.FirstOrDefault(i => i.Id == id);
		}

		public void UpdateImage(AdvertImageEntity image)
		{
			_context.AdvertImageEntities.Update(image);
			_context.SaveChanges();
		}
	}
}
