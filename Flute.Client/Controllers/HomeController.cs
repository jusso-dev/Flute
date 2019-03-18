using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flute.Client.Constants;
using Flute.Client.Interfaces;
using Flute.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace Flute.Client.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ITrainerService _trainerService;
		private readonly IBlobStorageService _blobService;
		public HomeController(ITrainerService trainerService, IBlobStorageService blobStorageService)
		{
			_trainerService = trainerService;
			_blobService = blobStorageService;
		}
		
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var claims = User.Claims;
			foreach(var item in claims)
			{
				
			}
			return View();
		}

		[Authorize]
		public IActionResult Training()
		{
			return View();
		}

		[Authorize]
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

				TempData["Info"] = "Model trained successfully. See 'Models' in the navbar, to consume you're newly trained model.";

				return View();
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();				
			}
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> TestPrediction([FromQuery] string modelId)
		{
			try
			{
				var blobs = await _blobService.ListBlobs(Shared.Services.BlobStorageService.TypeOfBlobUpload.ModelFile);

				return View(blobs);
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();
			}
		}
	}
}
