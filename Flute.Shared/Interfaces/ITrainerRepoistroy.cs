using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Flute.Shared.Models;

namespace Flute.Shared.Interfaces
{
	public interface ITrainerRepoistroy
	{
		Task<bool> InsertRecord(TrainedModelCosmosDB model);
		Task<bool> ReplaceDocument(TrainedModelCosmosDB model);
		Task<List<TrainedModelCosmosDB>> ListAllRecords(int numberOfItemsToTake);
		Task<bool> RemoveRecord(string id);
	}
}
