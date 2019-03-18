using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flute.Client.Interfaces
{
	public interface ITrainedModelRepoistory
	{
		Task<bool> AddNewModel(string modelId, string usersEmail);
		Task<int> CountModelsUploaded(string usersEmail);
		Task<bool> MaxAllowedModels(string usersEmail);
	}
}
