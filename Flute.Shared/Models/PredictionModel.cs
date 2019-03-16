using System;
using System.Collections.Generic;
using System.Text;

namespace Flute.Shared.Models
{
	/// <summary>
	/// Used for constructing object to generate ML prediction
	/// </summary>
	public class PredictionModel
	{
		public TrainedModel PredictionInput { get; set; }
		public string ModelId { get; set; }
	}
}
