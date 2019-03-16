using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Shared.Services;
using Flute.Trainer.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Flute.Trainer.Service.Controllers
{
	[Route("api/[controller]")]
    public class TrainerController : Controller
    {

		private readonly IMLTrainerService _trainerService;
		private readonly IBlobStorageService _blobService;
		public TrainerController(IMLTrainerService mLTrainerService, IBlobStorageService blobStorageService)
		{
			_trainerService = mLTrainerService;
			_blobService = blobStorageService;
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
				// Step 1: download file from blob storage for training
				var blobs = await _blobService.ListBlobs(BlobStorageService.TypeOfBlobUpload.TrainingFile);

				// Step 2: get reference to downloaded blob
				using (Stream stream = new MemoryStream())
				{
					var blobObject = await _blobService.DownloadBlob(blobs.FirstOrDefault(), stream);
					stream.Seek(0, SeekOrigin.Begin);

					using (var reader = new StreamReader(stream))
					using (var csv = new CsvReader(reader))
					{
						var records = csv.GetRecords<TrainedModel>();

						// Step 3: train model on objects
						var trainedModelSuccessfully = await _trainerService.BuildAndTrainModel(records);

						if (trainedModelSuccessfully)
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

		[HttpPost]
		[Route(nameof(Predict))]
		public async Task<IActionResult> Predict([FromBody] PredictionModel model)
		{
			try
			{
				if(string.IsNullOrEmpty(model?.ModelId))
				{
					return new JsonResult($"Model id {model?.ModelId ?? "Not found"} was not found")
					{
						StatusCode = 404
					};
				}
				
				var blob = await _blobService.ListBlobs(BlobStorageService.TypeOfBlobUpload.ModelFile);
				var oneBlob = blob
					.Where(a => a == model?.ModelId)
					.FirstOrDefault();

				var prediction = await _trainerService.UseModelWithSingleItem(model);

				if(!string.IsNullOrEmpty(oneBlob))
				{
					return new JsonResult(prediction)
					{
						StatusCode = 200
					};
				}
				else
				{
					return new JsonResult($"Model id {model?.ModelId} not found")
					{
						StatusCode = 404
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