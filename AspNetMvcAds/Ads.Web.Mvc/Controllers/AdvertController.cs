using Ads.Data;
using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
		private readonly IRepository<CategoryEntity> _categoryRepository;
		private readonly IRepository<AdvertEntity> _advertRepository;
		private readonly IRepository<CategoryAdvertEntity> _categoryAdvertRepository;
		private readonly IRepository<AdvertImageEntity> _advertImageRepository;
        private readonly IRepository<UserEntity> _userRepository;
		private readonly IRepository<AdvertCommentEntity> _advertCommentRepository;

		public AdvertController(IRepository<AdvertEntity> advertRepository,
								  IRepository<CategoryEntity> categoryRepository,
								  IRepository<CategoryAdvertEntity> categoryAdvertRepository,
								  IRepository<AdvertImageEntity> advertImageRepository,
                                  IRepository<UserEntity> userRepository,
								  IRepository<AdvertCommentEntity> advertCommentRepository)
		{
			_categoryRepository = categoryRepository;
			_advertRepository = advertRepository;
			_categoryAdvertRepository = categoryAdvertRepository;
			_advertImageRepository = advertImageRepository;
			_userRepository = userRepository;
			_advertCommentRepository = advertCommentRepository;
		}

		[Route("/advert/search")]
        public IActionResult Search(int page, string query)
        {
            ViewData["ListPartialTitle"] = query ?? "";
            return View();
        }

		[Route("/advert/detail/{id}")]
		public async Task<IActionResult> Detail(int id)
		{
			var advertResult=await _advertRepository.GetById(id);
			if (!advertResult.Success)
			{
				return NotFound();
			}
			var advertEntiy = advertResult.Data;
			var categoryAdvertResult = await _categoryAdvertRepository.GetAll()
				.FirstOrDefaultAsync(ca => ca.AdvertId == id);
			var categoryResult = await _categoryRepository.GetById(categoryAdvertResult?.CategoryId ?? 0);
			var userResult = await _userRepository.GetById(advertEntiy.UserId);
			if (!userResult.Success)
			{
				return NotFound();
			}
			var imagePath=await _advertImageRepository.GetAll()
				.Where(ai=>ai.AdvertId== id)
				.Select(ai=>ai.ImagePath)
				.ToListAsync();
			var comments = _advertCommentRepository.GetAll()
				.Where(c => c.AdvertId == id && c.IsActive)
				.Include(c => c.Advert)
				.ThenInclude(a=>a.User)
				.Select(c => new AdvertCommentViewModel
				{
					
					Id= c.Id,
					Comment=c.Comment,
					UserName=c.Advert.User.Name

				})
				.ToList() ;
			var advertModel = new AdvertDetailViewModel
			{
				Id = id,
				Title = advertEntiy.Title,
				Description = advertEntiy.Description,
				CreatedAt=advertEntiy.CreatedAt,
				CategoryId=categoryResult.Data?.Id??0,
				CategoryName=categoryResult.Data?.Name??"-",
				ImagePaths=imagePath,
				User=new UserViewModel
				{
					Id=userResult.Data.Id,
					Name=userResult.Data.Name,
					CreatedAt=userResult.Data.CreatedAt,
				},
				Price=advertEntiy.Price,
				AdvertComments=comments
				
			};
			return View(advertModel);

		}

		
	}
}
