using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Flute.Client.Interfaces;
using Flute.Shared.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Client.Controllers
{
    public class AccountController : Controller
    {
		private readonly IUserRepoistory _userRepo;
		public AccountController(IUserRepoistory userRepoistory)
		{
			_userRepo = userRepoistory;
		}
		public async Task<IActionResult> Login()
		{
			if (!HttpContext.User.Identity.IsAuthenticated)
			{
				return Challenge(GoogleDefaults.AuthenticationScheme);
			}

			// Will attempt to add new user when they login, if they exists, it return false
			var usersEmail = User.FindFirst(a => a.Type == ClaimTypes.Email)?.Value;
			await _userRepo.AddUser(usersEmail);

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Logout()
		{
			return SignOut(new AuthenticationProperties { RedirectUri = "/" },
				CookieAuthenticationDefaults.AuthenticationScheme);
		}
	}
}