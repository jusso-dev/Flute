using System.Collections.Generic;
using System.Threading.Tasks;
using Flute.Trainer.Service.Model;

namespace Flute.Trainer.Service.Interfaces
{
	public interface ITrainedModelRepoistory
	{
		Task<bool> AddNewModel(string modelId, string usersEmail);
		Task<int> CountModelsUploaded(string usersEmail);
		Task<bool> MaxAllowedModels(string usersEmail);
		Task<List<UsersUploadedModels>> ReturnUserModels(string usersEmail);
	}
}
