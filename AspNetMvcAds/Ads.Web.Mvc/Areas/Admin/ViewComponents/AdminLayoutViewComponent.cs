
using System.Security.Claims;
using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using Ads.Web.Mvc.Areas.Admin.Models;
using Ads.Web.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;


namespace Ads.Web.Mvc.Areas.Admin.ViewComponents

{
	public class AdminLayoutViewComponent:ViewComponent
	{
		private readonly IRepository<UserEntity> _userRepository;

		public AdminLayoutViewComponent(IRepository<UserEntity> userRepository)
		{
			_userRepository = userRepository;
		}

		public ViewViewComponentResult Invoke()
		{
			var activeUserCount = GetActiveUserCount();
			var usersCount = GetUserCount();
			var model = new AdminLayoutViewModel
			{
				ActiveUserCount = activeUserCount,
				UserCount = usersCount
			};
			return View(model);
		}

		private int GetActiveUserCount()
		{
			var activeUsers=_userRepository.GetAll().Count(u=>u.IsEmailConfirmed);
			return activeUsers;
		}

		private int GetUserCount()
		{
			var uCount = _userRepository.GetAll().Count();
			return uCount;
		}

	}
}
