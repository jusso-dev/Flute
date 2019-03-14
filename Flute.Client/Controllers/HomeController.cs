using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flute.Client.Models;
using Microsoft.AspNetCore.Http;
using Flute.Client.Constants;

namespace Flute.Client.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Training()
		{
			return View();
		}

		[HttpPost]
		public IActionResult TrainingPost(IFormFile file)
		{
			try
			{
				// Get reference to csv file, and validate file
				if(file.Length < 1)
				{
					TempData["Error"] = ViewConstants.FileEmptyMessage;
				}
				else if (file.Length > 200000)
				{
					TempData["Error"] = ViewConstants.FileLengthExceededMessage;
				}
				else if (!file.ContentType.Contains(".csv"))
				{
					TempData["Error"] = ViewConstants.FileWrongFormatMessage;
				}

				return View();
			}
			catch (Exception)
			{
				return View();				
			}
		}
	}
}
