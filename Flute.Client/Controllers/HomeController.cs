using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Flute.Client.Constants;
using Flute.Client.Interfaces;
using Flute.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Flute.Client.ViewModels;

namespace Flute.Client.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ITrainerService _trainerService;
		private readonly IBlobStorageService _blobService;
		private readonly IUserRepoistory _userRepo;
		private readonly ITrainedModelRepoistory _trainedModelRepo;
		public HomeController(ITrainerService trainerService, IBlobStorageService blobStorageService,
							 IUserRepoistory userRepoistory, ITrainedModelRepoistory trainedModelRepoistory)
		{
			_trainerService = trainerService;
			_blobService = blobStorageService;
			_userRepo = userRepoistory;
			_trainedModelRepo = trainedModelRepoistory;
		}
		
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult Training()
		{
			return View();
		}

		[Authorize]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<IActionResult> Training([FromForm] IFormFile file, [FromForm] string modelName)
		{
			try
			{
				if(string.IsNullOrEmpty(modelName) || modelName.Length > 20)
				{
					TempData["Error"] = "Please provide a meaningful name for your model and ensure the length is less than 20 characters.";
					return View();
				}

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

				var usersEmail = User.FindFirst(e => e.Type == ClaimTypes.Email)?.Value;

				// Check if user has uploaded maximum allow models
				bool maxModelAllowanceExceeded = _trainedModelRepo.MaxAllowedModels(usersEmail).Result;
				if(maxModelAllowanceExceeded)
				{
					TempData["Error"] = "You have reached the limit of models allowed (5) at this time.";
					return View();
				}
					
				var res = await _trainerService.QueueModel(new Shared.Models.ModelFile() { formFile = file, EmailAddress = usersEmail, ModelName = modelName });
				if(!res)
				{
					TempData["Error"] = "Failed to upload training file, please try again later. Please also check the format of your data meets the required format.";
					return View();
				}

				TempData["Info"] = "Model trained successfully. See 'Models' in the navbar, to consume your newly trained model.";
				return View();
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();				
			}
		}

		[Authorize]
		public async Task<IActionResult> Models()
		{
			try
			{
				var usersEmail = User.FindFirst(a => a.Type == ClaimTypes.Email)?.Value;

				// Return models for user
				var trainedModels = await _trainedModelRepo.ReturnUserModels(usersEmail);

				// Return user record from db
				var userRecord = await _userRepo.GetSingleUser(usersEmail);

				TrainedModelsViewModel viewModel = new TrainedModelsViewModel()
				{
					ListOfTrainedModels = trainedModels,
					UserRecord = userRecord
				};

				return View(viewModel);
			}
			catch (Exception ex)
			{
				TempData["Error"] = ex.Message;
				return View();
			}
		}
	}
}
