﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flute.Client.Constants;
using Flute.Client.Interfaces;
using Flute.Shared.Interfaces;

namespace Flute.Client.Controllers
{
	public class HomeController : Controller
	{
		private readonly ITrainerService _trainerService;
		private readonly IBlobStorageService _blobService;
		public HomeController(ITrainerService trainerService, IBlobStorageService blobStorageService)
		{
			_trainerService = trainerService;
			_blobService = blobStorageService;
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

				TempData["Info"] = "Model queued for training successfully.";

				return RedirectToAction(nameof(Training));
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();				
			}
		}

		[HttpGet]
		public async Task<IActionResult> ListTrainedModels()
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
