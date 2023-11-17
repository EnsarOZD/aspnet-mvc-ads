using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
	  [Area("Admin")]

    public class AdvertCommentController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertCommentController(AppDbContext context)
        {
            _context = context;
        }


		public async Task<IActionResult> Index(int? page)
		{
			int pageSize = 10;
			var comments = await _context.AdvertCommentEntities
				.Select(comment => new AdvertCommentsViewModel
				{
					Id = comment.Id,
					Comment = comment.Comment.Substring(0, 40) + "...",
					IsActive = comment.IsActive,
					CreatedAt = comment.CreatedAt,
					UpdatedAt = comment.UpdatedAt,
					DeletedAt = comment.DeletedAt,
				})
				.ToListAsync();

			int pageIndex = page ?? 1;
			var paginatedComments = comments.ToPagedList(pageIndex, pageSize);

			return View(paginatedComments);
		}

		public IActionResult Delete()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await DeleteCommentAsync(id);
            TempData["SuccessMessage"] = "Comment deleted succesfully";
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
		[Area("Admin")]
		public async Task<IActionResult> Edit(int id)
		{
			var comment = await _context.AdvertCommentEntities.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			var model = new AdvertCommentsViewModel
			{
				Id = comment.Id,
				Comment = comment.Comment,
				IsActive = comment.IsActive,
				CreatedAt = comment.CreatedAt,
				UpdatedAt = comment.UpdatedAt,
				DeletedAt = comment.DeletedAt,
				
			};

			return View(model);
		}

		//[Area("Admin")]
		[HttpPost]
		//[ValidateAntiForgeryToken] // CSRF koruması için eklendi
		public async Task<IActionResult> Edit(int id, AdvertCommentsViewModel model)
		{
			if (id != model.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				var comment = await _context.AdvertCommentEntities.FindAsync(id);
				if (comment == null)
				{
					return NotFound();
				}

				comment.Comment = model.Comment;
				comment.IsActive = model.IsActive;
				comment.UpdatedAt = DateTime.UtcNow; 

				try
				{
					_context.Update(comment);
					await _context.SaveChangesAsync();
					TempData["SuccessMessage"] = "Comment updated succesfully";
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!CommentExists(id))
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

			var errors=ModelState.Values.SelectMany(v=>v.Errors)
										.Select(e=>e.ErrorMessage)
										.ToList();
			ViewBag.Errors = errors;


			return View(model);
		}

		private bool CommentExists(int id)
		{
			return _context.AdvertCommentEntities.Any(e => e.Id == id);
		}

	}
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
        {
            int count = source.Count;
            List<T> items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

}
