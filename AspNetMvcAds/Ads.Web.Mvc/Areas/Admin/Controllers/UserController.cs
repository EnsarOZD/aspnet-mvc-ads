using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
		private readonly AppDbContext _context;
		public UserController(AppDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var user = await _context.UserEntities.Select(user => new UserViewModel
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				Phone = user.Phone,
				Roles = user.Roles,
				IsEmailConfirmed = user.IsEmailConfirmed,
				Address = user.Address,
				CreatedAt = user.DeletedAt,
			}).ToListAsync();
			return View(user);
		}
		public IActionResult Delete()
		{

			return View();
		}
		[Area("Admin")]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await DeleteCommentAsync(id);
			TempData["SuccessMessage"] = "User deleted succesfully";
			return RedirectToAction("Index");
		}
		public async Task DeleteCommentAsync(int userId)
		{
			var user = _context.UserEntities.Find(userId);
			if (user != null)
			{
				_context.UserEntities.Remove(user);
				await _context.SaveChangesAsync();
			}


		}
        public ActionResult ChangeEntityProperty(int entityId, string newValue)
        {
            // Find the entity by its ID or any other unique identifier.
            var entity = _context.UserEntities.Find(entityId);

            if (entity != null)
            {
                // Update the entity property with the new value.
                entity.Roles = newValue;

                // Save the changes to the database.
                _context.SaveChanges();

                // Redirect to a success page or return a response.
                return RedirectToAction("Index", "UserController");
            }
            else
            {
                // Handle the case where the entity is not found.
                return View("EntityNotFoundView");
            }
        }


    }

}

       
      
    

