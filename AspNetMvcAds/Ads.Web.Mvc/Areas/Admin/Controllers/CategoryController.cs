using Microsoft.AspNetCore.Mvc;
using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Data.Entities;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	
	[Area("Admin")]
    public class CategoryController : Controller
    {
		private readonly AppDbContext _context;
		public CategoryController(AppDbContext context)
		{
			_context = context;
		}

		
		public async Task<IActionResult> Index()
		{
			var category= await _context.CategoryEntities.Select(category => new AdminCategoryViewModel
			{
				Id = category.Id,
				Name = category.Name,
				Description = category.Description,
				UpdatedAt = category.UpdatedAt,
				CreatedAt = category.DeletedAt,
			}).ToListAsync();
			return View(category);
		}
	
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCategoryViewModel category)
        {
            if (ModelState.IsValid)
            {
                // Veritabanına yeni kategori ekleyin
                var newCategory = new CategoryEntity
                {
                    Name = category.Name,
                    Description = category.Description,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                _context.CategoryEntities.Add(newCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.CategoryEntities.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Models.AdminCategoryViewModel category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = await _context.CategoryEntities
				.FirstOrDefaultAsync(m => m.Id == id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
			
		}
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var category = await _context.CategoryEntities.FindAsync(id);
			_context.CategoryEntities.Remove(category);
			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
			
		}
		private bool CategoryExists(int id)
		{
			return _context.CategoryEntities.Any(c => c.Id == id);
		}


	}


}

