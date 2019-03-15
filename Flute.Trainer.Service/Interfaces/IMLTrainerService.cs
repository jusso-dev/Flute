using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flute.Trainer.Service.Model;
using Microsoft.ML;

namespace Flute.Trainer.Service.Interfaces
{
	public interface IMLTrainerService
	{
		Task<TrainedModelPrediction> UseModelWithSingleItem(ITransformer model, Flute.Shared.Models.TrainedModel sample);
		Task<ITransformer> GetReferenceToModel(Stream modelStream);
		Task<bool> BuildAndTrainModel(IEnumerable<Flute.Shared.Models.TrainedModel> trainedModels);
		Task<bool> SaveModelAsFile(ITransformer model);
	}
}
