using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            var comments = _context.AdvertCommentEntities.Select(comment => new AdvertCommentsViewModel
            {
                Id = comment.Id,
                Comment = comment.Comment.Substring(0, 40) + "...",
                IsActive = comment.IsActive,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                DeletedAt = comment.DeletedAt,
            }).ToList();
            return View(comments);
        }

        [Area("Admin")]
        public IActionResult Delete()
        {

            return View();
        }
        [Area("Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (DeleteComment(id))
            {
                ViewBag.SuccessMessage = "Comment has been deleted successfully.";
            }
            return RedirectToAction("Index");
        }
        public bool DeleteComment(int commentId)
        {
            var comment = _context.AdvertCommentEntities.Find(commentId);
            if (comment != null)
            {
                _context.AdvertCommentEntities.Remove(comment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


    }
}
