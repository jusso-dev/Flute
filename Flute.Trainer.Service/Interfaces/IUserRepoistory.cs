using System.Threading.Tasks;
using Flute.Trainer.Service.Model;

namespace Flute.Trainer.Service.Interfaces
{
	public interface IUserRepoistory
	{
		Task<AuthenticatedUserModel> GetSingleUser(string emailAddress);
		Task<bool> CheckForExistingUser(string emailAddress);
		Task<bool> AddUser(string emailAddress);
		Task<AuthenticatedUserModel> GetSingleUserByApiKey(string apiKey);
	}
}
