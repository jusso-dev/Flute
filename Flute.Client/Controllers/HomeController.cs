using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flute.Client.Models;
using Microsoft.AspNetCore.Http;
using Flute.Client.Constants;
using Flute.Client.Interfaces;

namespace Flute.Client.Controllers
{
	public class HomeController : Controller
	{
		private readonly ITrainerService _trainerService;
		public HomeController(ITrainerService trainerService)
		{
			_trainerService = trainerService;
		}
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Training()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Training(IFormFile file)
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
				else if (file.ContentType != "application/vnd.ms-excel")
				{
					TempData["Error"] = ViewConstants.FileWrongFormatMessage;
				}

				var res = await _trainerService.QueueModel(new Shared.Models.ModelFile() { formFile = file });
				if(!res)
				{
					TempData["Error"] = "Failed to upload Model, please try again later.";
				}

				return RedirectToAction(nameof(Training));
			}
			catch (Exception)
			{
				return View();				
			}
		}
	}
}
