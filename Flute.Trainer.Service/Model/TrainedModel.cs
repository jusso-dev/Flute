using Microsoft.ML.Data;

namespace Flute.Trainer.Service.Model
{
	public class TrainedModel
	{
		[LoadColumn(0)]
		public string Input { get; set; }
		[LoadColumn(1), ColumnName("Label")]
		public bool Label { get; set; }
	}

	public class TrainedModelPrediction
	{
		[ColumnName("PredictedLabel")]
		public bool Prediction { get; set; }
		
		public float Probability { get; set; }
		
		public float Score { get; set; }
	}
}
