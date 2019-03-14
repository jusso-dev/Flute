using System.Threading.Tasks;
using Flute.Shared.Models;

namespace Flute.Client.Interfaces
{
	public interface ITrainerService
	{
		Task<bool> QueueModel(ModelFile modelFile);
	}
}
