using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
using Flute.Shared.Services;
using Flute.Trainer.Service.Interfaces;
using Flute.Trainer.Service.Model;
using Microsoft.Data.DataView;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace Flute.Trainer.Service.Services
{
	public class MLTrainerService : IMLTrainerService
	{
		static MLContext mlContext = new MLContext();
		private readonly IBlobStorageService _blobService;
		public MLTrainerService(IBlobStorageService blobStorageService)
		{
			_blobService = blobStorageService;
		}

		public Task<TrainedModelPrediction> UseModelWithSingleItem(ITransformer model, Flute.Shared.Models.TrainedModel sample)
		{
			PredictionEngine<TrainedModel, TrainedModelPrediction> predictionFunction = model.CreatePredictionEngine<TrainedModel, TrainedModelPrediction>(mlContext);

			var resultprediction = predictionFunction.Predict(new TrainedModel() { Input = sample.Input, Label = Convert.ToBoolean(sample.Label) });

			return Task.FromResult(resultprediction);
		}

		public Task<ITransformer> GetReferenceToModel(Stream modelStream)
		{
			return Task.FromResult(mlContext.Model.Load(modelStream));
		}

		public async Task<bool> BuildAndTrainModel(IEnumerable<Flute.Shared.Models.TrainedModel> listOfTrainingObjects, string usersEmail)
		{
			try
			{
				List<TrainedModel> trainedModel = new List<TrainedModel>();
				foreach(var item in listOfTrainingObjects.ToList())
				{
					trainedModel.Add(new TrainedModel() { Input = item?.Input, Label = Convert.ToBoolean(item.Label) });
				}
				
				IDataView splitTrainSet = mlContext.Data.LoadFromEnumerable<TrainedModel>(trainedModel);

				var pipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: DefaultColumnNames.Features, inputColumnName: nameof(TrainedModel.Input))
				.Append(mlContext.BinaryClassification.Trainers.FastTree(numLeaves: 50, numTrees: 50, minDatapointsInLeaves: 20));

				var model = pipeline.Fit(splitTrainSet);

				await this.SaveModelAsFile(model, usersEmail);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<TrainedModelPrediction> UseModelWithSingleItem(Shared.Models.PredictionModel predictionModel, string usersEmail)
		{
			// Step 1: download file from blob storage
			var blobs = await _blobService.ListBlobs(BlobStorageService.TypeOfBlobUpload.ModelFile, usersEmail);
			var singleBlob = blobs
				.Where(a => a == predictionModel?.ModelId)
				.FirstOrDefault();

			// Step 2: get reference to downloaded blob
			using (Stream stream = new MemoryStream())
			{
				var blobObject = await _blobService.DownloadBlob(usersEmail, singleBlob, stream);
				stream.Seek(0, SeekOrigin.Begin);

				await blobObject.CopyToAsync(stream);

				var model = mlContext.Model.Load(stream);

				PredictionEngine<TrainedModel, TrainedModelPrediction> predictionFunction = model.CreatePredictionEngine<TrainedModel, TrainedModelPrediction>(mlContext);

				TrainedModel sampleStatement = new TrainedModel
				{
					Input = predictionModel.PredictionInput?.Input
				};

				var resultprediction = predictionFunction.Predict(sampleStatement);

				return resultprediction;
			}
		}

		public async Task<bool> SaveModelAsFile(ITransformer model, string usersEmail)
		{
			using (var stream = new MemoryStream())
			{
				mlContext.Model.Save(model, stream);
				stream.Seek(0, SeekOrigin.Begin);

				await _blobService.UploadBlob(stream, BlobStorageService.TypeOfBlobUpload.ModelFile, usersEmail:usersEmail);
			}

			return true;
		}
	}
}
