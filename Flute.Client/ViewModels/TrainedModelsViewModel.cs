using System.Collections.Generic;
using Flute.Client.Models;
using Flute.Shared.Models;

namespace Flute.Client.ViewModels
{
	public class TrainedModelsViewModel
	{
		public List<UsersUploadedModels> ListOfTrainedModels { get; set; }
		public AuthenticatedUserModel UserRecord { get; set; }
		public string CurrentHostName { get; set; }
	}
}
