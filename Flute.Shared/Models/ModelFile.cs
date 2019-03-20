using Microsoft.AspNetCore.Http;

namespace Flute.Shared.Models
{
	public class ModelFile
	{
		public IFormFile formFile { get; set; }
		public string EmailAddress { get; set; }
		public string ModelFriendlyName { get; set; }
	}
}
