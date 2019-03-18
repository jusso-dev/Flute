using System.Threading.Tasks;
using Flute.Client.Models;
using Flute.Shared.Models;

namespace Flute.Client.Interfaces
{
	public interface IUserRepoistory
	{
		Task<AuthenticatedUserModel> GetSingleUser(string emailAddress);
		Task<bool> CheckForExistingUser(string emailAddress);
		Task<bool> AddUser(string emailAddress);
	}
}
