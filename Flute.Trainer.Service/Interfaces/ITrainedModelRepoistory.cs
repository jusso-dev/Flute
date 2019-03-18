using System.Threading.Tasks;

namespace Flute.Trainer.Service.Interfaces
{
	public interface ITrainedModelRepoistory
	{
		Task<bool> AddNewModel(string modelId, string usersEmail);
		Task<int> CountModelsUploaded(string usersEmail);
		Task<bool> MaxAllowedModels(string usersEmail);
	}
}
