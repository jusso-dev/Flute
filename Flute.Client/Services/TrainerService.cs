using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Flute.Client.Interfaces;
using Flute.Shared;
using Flute.Shared.Models;

namespace Flute.Client.Services
{
	public class TrainerService : ITrainerService
	{
		private readonly IConfigurationReader _config;
		private readonly IHttpClientFactory _client;

		private string ApiBaseUrl = string.Empty;
		public TrainerService(IConfigurationReader configurationReader, IHttpClientFactory httpClientFactory)
		{
			_config = configurationReader;
			_client = httpClientFactory;

			ApiBaseUrl = _config.ReadConfigurationAsync("TrainServiceBaseUrl", false).Result;
		}

		/// <summary>
		/// Send model stream to Flute.Trainer.Service so that it can be trained
		/// </summary>
		/// <returns></returns>
		public async Task QueueModel(ModelFile modelFile)
		{
			try
			{
				var httpclient = _client.CreateClient();
				var res = httpclient.PostAsync<ModelFile>(ApiBaseUrl + "/TrainerService", modelFile, );
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
