using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{

    public class AdvertCommentController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertCommentController(AppDbContext context)
        {
            _context = context;
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var comments = await _context.AdvertCommentEntities.Select(comment => new AdvertCommentsViewModel
            {
                Id = comment.Id,
                Comment = comment.Comment.Substring(0, 40) + "...",
                IsActive = comment.IsActive,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                DeletedAt = comment.DeletedAt,
            }).ToListAsync();
            return View(comments);
        }

        [Area("Admin")]
        public IActionResult Delete()
        {

            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await DeleteCommentAsync(id);
            return RedirectToAction("Index");
        }
        public async Task DeleteCommentAsync(int commentId)
        {
            var comment = _context.AdvertCommentEntities.Find(commentId);
            if (comment != null)
            {
                _context.AdvertCommentEntities.Remove(comment);
                await _context.SaveChangesAsync();
            }


        }
    }
}
