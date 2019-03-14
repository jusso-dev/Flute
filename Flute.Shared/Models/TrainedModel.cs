using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Flute.Shared.Models
{
	public class TrainedModel
	{
		public string Input { get; set; }
		public float Label { get; set; }
	}
}
