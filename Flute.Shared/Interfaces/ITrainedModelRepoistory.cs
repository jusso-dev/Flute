using System.Collections.Generic;
using System.Threading.Tasks;
using Flute.Shared.Models;

namespace Flute.Shared.Interfaces
{
	public interface ITrainedModelRepoistory
	{
		Task<bool> AddNewModel(string modelId, string usersEmail, string modelFriendlyName);
		Task<UsersUploadedModels> CountModelsUploaded(string usersEmail);
		Task<bool> MaxAllowedModels(string usersEmail);
		Task<List<UsersUploadedModels>> ReturnUserModels(string usersEmail);
	}
}
