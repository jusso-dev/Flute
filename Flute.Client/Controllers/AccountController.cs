using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Client.Controllers
{
    public class AccountController : Controller
    {
		public IActionResult Login()
		{
			if (!HttpContext.User.Identity.IsAuthenticated)
			{
				return Challenge(GoogleDefaults.AuthenticationScheme);
			}

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Logout()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				return SignOut(CookieAuthenticationDefaults.AuthenticationScheme, GoogleDefaults.AuthenticationScheme);
			}

			return RedirectToAction("Index", "Home");
		}
	}
}