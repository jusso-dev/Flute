using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
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

			var resultprediction = predictionFunction.Predict(new TrainedModel() { Input = sample.Input, Label = sample.Label });

			return Task.FromResult(resultprediction);
		}

		public Task<ITransformer> GetReferenceToModel(Stream modelStream)
		{
			return Task.FromResult(mlContext.Model.Load(modelStream));
		}

		public async Task<bool> BuildAndTrainModel(IEnumerable<Flute.Shared.Models.TrainedModel> trainedModels)
		{
			try
			{
				List<TrainedModel> trainedModel = new List<TrainedModel>();
				foreach(var item in trainedModels)
				{
					trainedModel.Add(new TrainedModel() { Input = item?.Input, Label = item.Label });
				}


				IDataView splitTrainSet = mlContext.Data.LoadFromEnumerable<TrainedModel>(trainedModel);

				var pipeline = mlContext.Transforms.Text.FeaturizeText(outputColumnName: DefaultColumnNames.Features, inputColumnName: nameof(TrainedModel.Input))
				.Append(mlContext.BinaryClassification.Trainers.FastTree(numLeaves: 50, numTrees: 50, minDatapointsInLeaves: 20));

				var model = pipeline.Fit(splitTrainSet);

				await this.SaveModelAsFile(model);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<bool> SaveModelAsFile(ITransformer model)
		{
			using (var fs = new MemoryStream())
			{
				mlContext.Model.Save(model, fs);
				await _blobService.UploadBlob(fs);
			}

			return true;
		}
	}
}
