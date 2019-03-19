using System.Threading.Tasks;
using Flute.Shared.Models;

namespace Flute.Shared.Interfaces
{
	public interface IUserRepoistory
	{
		Task<AuthenticatedUserModel> GetSingleUser(string emailAddress);
		Task<bool> CheckForExistingUser(string emailAddress);
		Task<bool> AddUser(string emailAddress);
		Task<AuthenticatedUserModel> GetSingleUserByApiKey(string apiKey);
	}
}
