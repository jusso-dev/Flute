using System;
using System.Collections.Generic;
using System.Text;

namespace Flute.Shared.Models
{
	/// <summary>
	/// POCO that is posted by Flute.Client to backend, to determine what information is need to store training file
	/// and model file
	/// </summary>
	public class UserModelToTrain
	{
		public string EmailAddress { get; set; }
		public string ModelFriendlyName { get; set; }
	}
}
