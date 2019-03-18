using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flute.Trainer.Service.Model
{
	public class AuthenticatedUserModel
	{
		public int Id { get; set; }
		public string EmailAddress { get; set; }
		public DateTime LastLogin { get; set; }
		public string ApiKey { get; set; }

	}

	public class UsersUploadedModels
	{
		public int Id { get; set; }
		public string EmailAddress { get; set; }
		public string ModelId { get; set; }
		public int UploadedModelCount { get; set; }
		public DateTime ModelUploadDateTime { get; set; }
	}
}
