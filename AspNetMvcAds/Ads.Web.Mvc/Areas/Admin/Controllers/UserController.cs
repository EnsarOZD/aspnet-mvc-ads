﻿using Ads.Data;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Bogus.DataSets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using X.PagedList;

namespace Ads.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "RequireAdministratorRole")]

    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? page)
        {

            int pageSize = 10;
            var userViewModels = new List<AdminUserViewModel>();
            var viewmodel = await _context.UserEntities.Select(user => new AdminUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Roles = user.Roles,
                    IsEmailConfirmed = user.IsEmailConfirmed,
                    Address = user.Address,
                    CreatedAt = user.CreatedAt,
                }).ToListAsync();
            userViewModels.AddRange(viewmodel);
            int pageIndex = page ?? 1;
            var pagedUsers = userViewModels.ToPagedList(pageIndex, pageSize);
            return View(pagedUsers);


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
        public IActionResult Edit([FromRoute] int id)
        {
            var user = _context.UserEntities.Find(id);
            var userViewModel = new AdminUserViewModel
            {
                Id = id,
                Name = user.Name,
                Email = user.Email,
                Address = user.Address,
                IsEmailConfirmed = user.IsEmailConfirmed,
                Phone = user.Phone,

            };
            return View(userViewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute] int id, UserRoles roles, AdminUserViewModel userViewModel)
        {
            var user = await _context.UserEntities.FindAsync(id);

            if (user != null)
            {
                user.Name = userViewModel.Name;
                user.Email = userViewModel.Email;
                user.Address = userViewModel.Address??null;
                user.IsEmailConfirmed = userViewModel.IsEmailConfirmed;
                user.Phone = userViewModel.Phone;
                user.Roles = roles.ToString();

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

    }
}


