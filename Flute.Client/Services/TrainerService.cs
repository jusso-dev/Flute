using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flute.Client.Interfaces;
using Flute.Shared;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Flute.Shared.Services;

namespace Flute.Client.Services
{
	public class TrainerService : ITrainerService
	{
		private readonly IConfigurationReader _config;
		private readonly IHttpClientFactory _client;
		private readonly IBlobStorageService _blobStorageService;

		private string ApiBaseUrl = string.Empty;
		public TrainerService(IConfigurationReader configurationReader, IHttpClientFactory httpClientFactory,
							  IBlobStorageService blobStorageService)
		{
			_config = configurationReader;
			_client = httpClientFactory;
			_blobStorageService = blobStorageService;

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
				// Step 1: Write file to blob storage
				using(Stream stream = new MemoryStream())
				{
					await modelFile.formFile.CopyToAsync(stream);
					stream.Seek(0, SeekOrigin.Begin);

					await _blobStorageService.UploadBlob(stream:stream, typeOfBlobUpload:BlobStorageService.TypeOfBlobUpload.TrainingFile, usersEmail:modelFile?.EmailAddress, fileName:modelFile?.formFile?.FileName);
				}

				// Step 2: Kick off training by triggering backend, might change this to be redis, not sure yet...
				var httpclient = _client.CreateClient();

				var res = await httpclient.PostAsJsonAsync<UserModelToTrain>(ApiBaseUrl + "/Trainer/TrainModel", new UserModelToTrain()
				{   EmailAddress = modelFile?.EmailAddress,
					ModelFriendlyName = modelFile?.ModelFriendlyName
				});

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
