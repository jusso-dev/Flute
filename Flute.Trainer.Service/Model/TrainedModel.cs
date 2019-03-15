using Microsoft.ML.Data;

namespace Flute.Trainer.Service.Model
{
	public class TrainedModel
	{
		[LoadColumn(0)]
		public string Input { get; set; }
		[LoadColumn(1), ColumnName("Label")]
		public float Label { get; set; }
	}

	public class TrainedModelPrediction
	{
		[ColumnName("PredictedLabel")]
		public float Prediction { get; set; }

		[ColumnName("Probability")]
		public float Probability { get; set; }

		[ColumnName("Score")]
		public float Score { get; set; }
	}
}
