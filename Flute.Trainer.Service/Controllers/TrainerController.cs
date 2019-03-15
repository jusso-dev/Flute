using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Trainer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Trainer.Service.Controllers
{
	[Route("api/[controller]")]
    public class TrainerController : Controller
    {

		private readonly IMLTrainerService _trainerService;
		private readonly ITrainerRepoistroy _trainerRepo;
		public TrainerController(IMLTrainerService mLTrainerService, ITrainerRepoistroy trainerRepoistroy)
		{
			_trainerService = mLTrainerService;
			_trainerRepo = trainerRepoistroy;
		}

		/// <summary>
		/// API endpoint that will trigger the training of the last model
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[Route(nameof(TrainModel))]
		public async Task<IActionResult> TrainModel()
		{
			try
			{
				// Step 1: get reference to all objects in database for training
				var modelObjects = _trainerRepo.ListAllRecords(1000).Result;

				// Step 2: Convert dto objects to objects that the trainer can accept
				List<TrainedModel> listOfTrainerObjects = new List<TrainedModel>();
				modelObjects.ForEach(a =>
				{
					listOfTrainerObjects.Add(new TrainedModel() { Input = a.Input, Label = a.Label } );
				});

				// Step 3: train model on objects
				var trainedModelSuccessfully = await _trainerService.BuildAndTrainModel(listOfTrainerObjects);

				if(trainedModelSuccessfully)
				{
					return new OkObjectResult("Accepted. Training commencing..");
				}
				else
				{
					return new JsonResult("Something went wrong.")
					{
						StatusCode = 500
					};
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(ex.Message)
				{
					StatusCode = 500
				};
			}
		}
	}
}