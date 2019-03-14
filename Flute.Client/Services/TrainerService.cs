using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using Flute.Client.Interfaces;
using Flute.Shared;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;

namespace Flute.Client.Services
{
	public class TrainerService : ITrainerService
	{
		private readonly IConfigurationReader _config;
		private readonly IHttpClientFactory _client;
		private readonly ITrainerRepoistroy _trainerRepo;

		private string ApiBaseUrl = string.Empty;
		public TrainerService(IConfigurationReader configurationReader, IHttpClientFactory httpClientFactory,
							  ITrainerRepoistroy trainerRepoistroy)
		{
			_config = configurationReader;
			_client = httpClientFactory;
			_trainerRepo = trainerRepoistroy;

			ApiBaseUrl = _config.ReadConfigurationAsync(ConfigurationReader.TrainerBaseApiUrl, false).Result;
		}

		/// <summary>
		/// Write to model to the database then trigger the backend to kick off training the model
		/// </summary>
		/// <returns></returns>
		public async Task<bool> QueueModel(ModelFile modelFile)
		{
			try
			{
				// Step 1: Write file contents to database
				using(var streamContent = modelFile.formFile.OpenReadStream())
				using (var reader = new StreamReader(streamContent))
				using (var csv = new CsvReader(reader))
				{
					csv.Configuration.HasHeaderRecord = true;
					var records = csv.GetRecords<TrainedModel>();

					foreach (var item in records)
					{
						await _trainerRepo.InsertRecord(new TrainedModelCosmosDB() { Input = item.Input, Label = item.Label });
					}
				}

				// Step 2: Kick off training by triggering backend, might change this to be redis, not sure yet...
				var httpclient = _client.CreateClient();
				var res = await httpclient.GetAsync(ApiBaseUrl + "/Trainer/TrainModel");

				if(res.StatusCode == HttpStatusCode.OK)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
