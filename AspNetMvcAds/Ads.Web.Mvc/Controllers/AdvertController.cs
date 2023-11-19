using Ads.Data;
using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
		private readonly AppDbContext _dbContext;

		public AdvertController(IRepository<AdvertEntity> advertRepository,
								  IRepository<CategoryEntity> categoryRepository,
								  IRepository<CategoryAdvertEntity> categoryAdvertRepository,
								  IRepository<AdvertImageEntity> advertImageRepository,
                                  IRepository<UserEntity> userRepository,
								  IRepository<AdvertCommentEntity> advertCommentRepository,
								  AppDbContext dbContext)
		{
			_categoryRepository = categoryRepository;
			_advertRepository = advertRepository;
			_categoryAdvertRepository = categoryAdvertRepository;
			_advertImageRepository = advertImageRepository;
			_userRepository = userRepository;
			_advertCommentRepository = advertCommentRepository;
			_dbContext = dbContext;
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
			advertEntiy.AdvertClickCount += 1;
			await _advertRepository.Update(advertEntiy);
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
				.ToList()
				.Select(c => new AdvertCommentViewModel
				{
					
					Id= c.Id,
					Comment=c.Comment,
					UserName=_userRepository.GetAll().FirstOrDefault(u=>u.Id==c.UserId)?.Name,
					CreatedAt=c.CreatedAt

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
					Id= userResult.Data.Id,
					Name=userResult.Data.Name,
					CreatedAt=userResult.Data.CreatedAt,
					NewEmail = userResult.Data.Email,
					Phone = userResult.Data.Phone
				},
				Price=advertEntiy.Price,
				AdvertComments=comments
				
			};
			return View(advertModel);

		}


		[HttpPost]
        public async Task<IActionResult> Comment(int Id, string review)
        {
			
            var advertEntityResult = await _advertRepository.GetById(Id);
            if (!advertEntityResult.Success)
            {
                return NotFound();
            }
			var advertEntity=advertEntityResult.Data;
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var userIdClaim=claimsIdentity?.FindFirst(ClaimTypes.PrimarySid);
			var userId=userIdClaim?.Value;
			
           
			var newComment = new AdvertCommentEntity			
			{
				AdvertId = Id,
				Comment= review,
				CreatedAt = DateTime.Now,
				IsActive=false,
				UserId=int.Parse(userId)

			};
			await _advertCommentRepository.Add(newComment);
			await _dbContext.SaveChangesAsync();
			TempData["SuccessMessage"] = "Advert comment added succesfully";

			return RedirectToAction("Detail", new { id = Id });
		}
    }
    }
