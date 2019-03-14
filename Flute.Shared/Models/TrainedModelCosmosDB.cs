using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Flute.Shared.Models
{
	public class TrainedModelCosmosDB
	{
		public string Input { get; set; }
		public float Label { get; set; }
		[JsonProperty("id")]
		public string Id { get; set; }
		[JsonProperty("_ts")]
		public string TimeStamp { get; set; }
	}
}
