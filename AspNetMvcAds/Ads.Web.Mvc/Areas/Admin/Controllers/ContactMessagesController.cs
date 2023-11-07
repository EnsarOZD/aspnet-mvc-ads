using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactMessagesController : Controller
    {
        private readonly AppDbContext _context;
        public ContactMessagesController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var messages = await _context.ContactUsMessage.Select(messages => new ContactMessagesViewModel
            {
                ContactId = messages.ContactId,
                Name = messages.Name,
                Email = messages.Email,
                Category = messages.Category,
                Context = messages.Context,
                DateTime = messages.DateTime,
                Status = messages.Status,
            }).ToListAsync();

            return View(messages);
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
            TempData["SuccessMessage"] = "Message deleted succesfully";
            return RedirectToAction("Index");
        }
        public async Task DeleteCommentAsync(int messageId)
        {
            var message = _context.ContactUsMessage.Find(messageId);
            if (message != null)
            {
                _context.ContactUsMessage.Remove(message);
                await _context.SaveChangesAsync();
            }


        }
    }
}


