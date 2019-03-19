using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Shared.Services;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Model;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Trainer.Service.Controllers
{
	[Route("api/[controller]")]
    public class TrainerController : Controller
    {

		private readonly IMLTrainerService _trainerService;
		private readonly IBlobStorageService _blobService;
		private readonly IUserRepoistory _userRepo;
		private readonly ITrainedModelRepoistory _trainedModelRepo;
		public TrainerController(IMLTrainerService mLTrainerService, IBlobStorageService blobStorageService,
								IUserRepoistory userRepoistory, ITrainedModelRepoistory trainedModelRepoistory)
		{
			_trainerService = mLTrainerService;
			_blobService = blobStorageService;
			_userRepo = userRepoistory;
			_trainedModelRepo = trainedModelRepoistory;
		}

		/// <summary>
		/// API endpoint that will trigger the training of the last model
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[Route(nameof(TrainModel))]
		public async Task<IActionResult> TrainModel([FromBody] UserModelToTrain userModelToTrain)
		{
			try
			{
				if(string.IsNullOrEmpty(userModelToTrain?.EmailAddress) || userModelToTrain == null)
				{
					return new JsonResult(new ControllerResponse() { Message = "UserModelToTrain model was null or missing email address", ResponseCode = 400 })
					{
						StatusCode = 400
					};
				}

				// Step 1: download file from blob storage for training
				var blobs = await _blobService.ListBlobs(BlobStorageService.TypeOfBlobUpload.TrainingFile, userModelToTrain?.EmailAddress);

				// Step 2: get reference to downloaded blob
				using (Stream stream = new MemoryStream())
				{
					var blobObject = await _blobService.DownloadBlob(userModelToTrain?.EmailAddress, blobs.FirstOrDefault(), stream);
					stream.Seek(0, SeekOrigin.Begin);

					using (var reader = new StreamReader(stream))
					using (var csv = new CsvReader(reader))
					{
						var records = csv.GetRecords<Shared.Models.TrainedModel>().ToList();

						// Step 3: train model on objects
						var trainedModelSuccessfully = await _trainerService.BuildAndTrainModel(records, userModelToTrain?.EmailAddress);

						if (trainedModelSuccessfully)
						{
							// Commit model id to users record in db
							string modelId = _blobService.ListBlobs(BlobStorageService.TypeOfBlobUpload.ModelFile, userModelToTrain?.EmailAddress).Result.FirstOrDefault();
							await _trainedModelRepo.AddNewModel(modelId, userModelToTrain?.EmailAddress, userModelToTrain?.ModelFriendlyName);

							// Delete any training files for user
							await _blobService.RemoveTrainingFile(userModelToTrain?.EmailAddress);

							return new OkObjectResult(new ControllerResponse() { Message = "Accepted. Training completed successfully.", ResponseCode = 200 });
						}
						else
						{
							return new JsonResult(new ControllerResponse() { Message = "Something went wrong.", ResponseCode = 500 })
							{
								StatusCode = 500
							};
						}
					}
				}
			}
			catch (Exception ex)
			{
				return new JsonResult(new ControllerResponse() { Message = "An error occurred training, please try again later.", ResponseCode = 500 })
				{
					StatusCode = 500
				};
			}
		}

		[HttpPost]
		[Route(nameof(Predict))]
		public async Task<IActionResult> Predict([FromBody] PredictionModel model)
		{
			try
			{
				var apiKeyHeader = Request.Headers["api-key"];
				if (apiKeyHeader.Any() == false)
				{
					return new JsonResult(new ControllerResponse() { Message = "api-key header was missing", ResponseCode = 401 })
					{
						StatusCode = 401
					};
				}

				// Check if user's api key exists against record
				var userRecord = _userRepo.GetSingleUserByApiKey(apiKeyHeader.ToString()).Result;
				if(string.IsNullOrEmpty(userRecord?.EmailAddress))
				{
					return new JsonResult(new ControllerResponse() { Message = "User record not found.", ResponseCode = 400 })
					{
						StatusCode = 400
					};
				}

				if(string.IsNullOrEmpty(model?.ModelId))
				{
					return new JsonResult(new ControllerResponse() { Message = $"Model id {model?.ModelId ?? "Not found"} was not found", ResponseCode = 404 })
					{
						StatusCode = 404
					};
				}

				var prediction = await _trainerService.UseModelWithSingleItem(model, userRecord?.EmailAddress);

				if(prediction != null)
				{
					return new JsonResult(new TrainedModelOutput() { Input = model.PredictionInput.Input, Label = Convert.ToInt32(prediction.Prediction), ModelId = model.ModelId, Score = prediction.Probability })
					{
						StatusCode = 200
					};
				}
				else
				{
					return new JsonResult(new ControllerResponse() { Message = $"Model id {model?.ModelId} not found", ResponseCode = 404 })
					{
						StatusCode = 404
					};
				}
			}
			catch (Exception)
			{
				return new JsonResult(new ControllerResponse() { Message = "An error occurred whilst retrieving a prediction, please try again later.", ResponseCode = 500 })
				{
					StatusCode = 500
				};
			}
		}
	}
}