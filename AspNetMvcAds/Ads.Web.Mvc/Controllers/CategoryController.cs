using Ads.Data;
using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace Ads.Web.Mvc.Controllers
{
	public class CategoryController : Controller
	{
		private readonly IRepository<CategoryEntity> _categoryRepository;
		private readonly IRepository<AdvertEntity> _advertRepository;
		private readonly IRepository<CategoryAdvertEntity> _categoryAdvertRepository;
		private readonly IRepository<AdvertImageEntity> _advertImageRepository;

		public CategoryController(IRepository<AdvertEntity> advertRepository,
								  IRepository<CategoryEntity> categoryRepository,
								  IRepository<CategoryAdvertEntity> categoryAdvertRepository,
								  IRepository<AdvertImageEntity> advertImageRepository)
		{
			_categoryRepository = categoryRepository;
			_advertRepository = advertRepository;
			_categoryAdvertRepository = categoryAdvertRepository;
			_advertImageRepository = advertImageRepository;
		}

		[Route("/category/")]
		[Route("/category/{category-slug}")]
		public async Task<IActionResult> Index(int? id, int page = 1)
		{


			ViewData["Page"] = page;
			int skipCount = (page - 1) * 12;
			int pageSize = 12;
			if (!id.HasValue||id==0)
			{
				var totalAdvertCount = await _advertRepository.GetAll().CountAsync();
				var totalPageCount = (int)Math.Ceiling(totalAdvertCount / (double)pageSize);
				ViewData["TotalPageCount"] = totalPageCount;
				ViewData["ListPartialTitle"] = "All Adverts";

				var allAdvertEntites = await _advertRepository.GetAll()
					.Skip(skipCount)
					.Take(12)
					.ToListAsync();
				var allAdvertIds = allAdvertEntites.Select(a => a.Id).ToList();

				var allAdvertImageEntites = await _advertImageRepository.GetAll()
					.Where(x => allAdvertIds.Contains(x.AdvertId))
					.ToListAsync();
				var allCategoryAdverts = await _categoryAdvertRepository.GetAll()
					.Where(ca => allAdvertIds.Contains(ca.AdvertId))
					.ToListAsync();

				var allCategoryIds = allCategoryAdverts.Select(ca => ca.CategoryId).Distinct().ToList();
				var allCategories = await _categoryRepository.GetAll()
					.Where(c => allCategoryIds.Contains(c.Id))
					.ToListAsync();
				

				var advertModelList = allAdvertEntites.Select(x =>
				{
					var relatedCategories = allCategoryAdverts.Where(ca => ca.AdvertId == x.Id).ToList();

					return new AdvertViewModel
					{
						Id = x.Id,
						Title = x.Title,
						Description = x.Description,
						CreatedAt = x.CreatedAt,
						ImagePath = allAdvertImageEntites.FirstOrDefault(p => p.AdvertId == x.Id)?.ImagePath ?? "",
						CategoryIds = relatedCategories.Select(rc => rc.CategoryId).ToList(),
						CategoryNames = relatedCategories.Select(rc => allCategories.FirstOrDefault(c => c.Id == rc.CategoryId)?.Name).ToList()
					};
				}).ToList();

				return View(advertModelList);
			}

			else
			{
				var categoryAdvertEntites = await _categoryAdvertRepository.GetAll()
				.Where(x => x.CategoryId == id.Value)
				.Select(x => x.AdvertId)
				.ToListAsync();

				var totalPageCount = (int)Math.Ceiling(categoryAdvertEntites.Count / (double)pageSize);
				ViewData["TotalPageCount"] = totalPageCount;
				var categoryResult = await _categoryRepository.GetById(id.Value);
				if (!categoryResult.Success)
				{
					return BadRequest("Invalid request");
				}

				var category = categoryResult.Data;
				ViewData["CategoryId"] = id;
				ViewData["ListPartialTitle"] = category.Name;

			
				if (!categoryAdvertEntites.Any())
				{
					return View(new List<AdvertViewModel>());
				}

				var advertEntites = await _advertRepository.GetAll()
					.Where(x => categoryAdvertEntites.Contains(x.Id))
					.Skip(skipCount)
					.Take(12)
					.ToListAsync();
				var advertImageEntites = await _advertImageRepository.GetAll()
					.Where(x => categoryAdvertEntites.Contains(x.AdvertId))
					.ToListAsync();
				var advertModelList = advertEntites.Select(x => new AdvertViewModel
				{
					Id = x.Id,
					Title = x.Title,
					Description = x.Description,
					CreatedAt = x.CreatedAt,
					CategoryId = category.Id,
					CategoryName = category.Name,
					ImagePath = advertImageEntites.FirstOrDefault(p => p.AdvertId == x.Id)?.ImagePath ?? ""
				}).ToList();

				return View(advertModelList);
			}

		}
	}
}
