using Microsoft.AspNetCore.Mvc;
using Ads.Data;
using Ads.Web.Mvc.Models;
using Microsoft.EntityFrameworkCore;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Data.Entities;
using X.PagedList;

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


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            var categoryViewModels = new List<AdminCategoryViewModel>();

            var categories = await _context.CategoryEntities
                .Select(category => new AdminCategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    UpdatedAt = category.UpdatedAt,
                    CreatedAt = category.CreatedAt, 
                })
                .ToListAsync();

            categoryViewModels.AddRange(categories);

            int pageIndex = page ?? 1;
            var pagedCategories = categoryViewModels.ToPagedList(pageIndex, pageSize);
            return View(pagedCategories);
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
                return Redirect("/admin/category");
            }

            return View(category);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _context.CategoryEntities.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var adminCategoryViewModel = new AdminCategoryViewModel()
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt

            };

            return View(adminCategoryViewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminCategoryViewModel adminCategoryViewModel)
        {
            if (id != adminCategoryViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _context.CategoryEntities.FindAsync(id);

                    if (category == null)
                    {
                        return NotFound();
                    }

                    category.Name = adminCategoryViewModel.Name;
                    category.Description = adminCategoryViewModel.Description;
                    category.UpdatedAt = DateTime.Now;

                    _context.Update(category);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Category", new { area = "Admin" });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(adminCategoryViewModel);
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
            return Redirect("/admin/category");

        }
        private bool CategoryExists(int id)
        {
            return _context.CategoryEntities.Any(c => c.Id == id);
        }


    }


}

