using System.Collections.Generic;
using Flute.Client.Models;

namespace Flute.Client.ViewModels
{
	public class TrainedModelsViewModel
	{
		public List<UsersUploadedModels> ListOfTrainedModels { get; set; }
		public AuthenticatedUserModel UserRecord { get; set; }
	}
}
