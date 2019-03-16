﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flute.Trainer.Service.Model;
using Microsoft.ML;

namespace Flute.Trainer.Service.Interfaces
{
	public interface IMLTrainerService
	{
		Task<ITransformer> GetReferenceToModel(Stream modelStream);
		Task<bool> BuildAndTrainModel(IEnumerable<Flute.Shared.Models.TrainedModel> trainedModels);
		Task<bool> SaveModelAsFile(ITransformer model);
		Task<TrainedModelPrediction> UseModelWithSingleItem(Shared.Models.PredictionModel predictionModel);
	}
}
