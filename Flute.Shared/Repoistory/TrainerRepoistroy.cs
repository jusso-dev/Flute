using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
using Flute.Shared.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;

namespace Flute.Shared.Repoistory
{
	/// <summary>
	/// Houses training data to train Binary Neural networks
	/// </summary>
	public class TrainerRepoistroy : ITrainerRepoistroy
	{
		public string EndpointUri;
		public string PrimaryKey;

		private const string databaseName = "TrainerDB";
		private const string collectionName = "TrainerCollection";

		private static DocumentClient _client;
		private readonly ILogger<TrainerRepoistroy> _log;
		private readonly IConfigurationReader _config;
		public TrainerRepoistroy(ILogger<TrainerRepoistroy> loggingService, IConfigurationReader config)
		{
			_log = loggingService ?? throw new ArgumentNullException($"{nameof(ILogger<TrainerRepoistroy>)} is null.");
			_config = config ?? throw new ArgumentNullException($"{nameof(IConfigurationReader)} is null.");

#if DEBUG
			//Set local Azure Cosmos DB Emulator keys
			EndpointUri = "https://localhost:8081";
			PrimaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
#else
			EndpointUri = _config.ReadConfigurationAsync(keyName:"CosmosDBConnectionString", readFromKeyVault:false).Result;
			PrimaryKey = _config.ReadConfigurationAsync(keyName:nameof(PrimaryKey), readFromKeyVault:true).Result;
#endif
			_client = new DocumentClient(new Uri(EndpointUri), PrimaryKey, new ConnectionPolicy() { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });

			_client.CreateDatabaseIfNotExistsAsync(new Database() { Id = databaseName }).ConfigureAwait(false);
			_client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName), new DocumentCollection() { Id = collectionName }).ConfigureAwait(false);

			_client.OpenAsync().ConfigureAwait(false);
		}

		public async Task<bool> InsertRecord(TrainedModelCosmosDB model)
		{
			try
			{
				var result = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), model);
				if (result.StatusCode == System.Net.HttpStatusCode.OK || result.StatusCode == System.Net.HttpStatusCode.Created)
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
				_log.LogError(ex.Message, ex);
				throw ex;
			}
		}

		public async Task<bool> ReplaceDocument(TrainedModelCosmosDB model)
		{
			try
			{
				List<TrainedModelCosmosDB> userQuery = _client.CreateDocumentQuery<TrainedModelCosmosDB>(
						UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), new FeedOptions() { MaxItemCount = 1 })
						.Where(a => a.Id == model.Id)
						.Take(1)
						.ToList();

				model.Id = userQuery.FirstOrDefault().Id;

				var res = await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, model.Id), model);
				if (res.StatusCode == System.Net.HttpStatusCode.Accepted || res.StatusCode == System.Net.HttpStatusCode.OK || res.StatusCode == System.Net.HttpStatusCode.Created)
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
				_log.LogError(ex.Message, ex);
				throw ex;
			}
		}

		public Task<List<TrainedModelCosmosDB>> ListAllRecords(int numberOfItemsToTake)
		{
			try
			{
				List<TrainedModelCosmosDB> userQuery = _client.CreateDocumentQuery<TrainedModelCosmosDB>(
					UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), new FeedOptions() { MaxItemCount = numberOfItemsToTake })
					.OrderByDescending(a => a.TimeStamp)
					.Take(numberOfItemsToTake)
					.ToList();

				return Task.FromResult(userQuery);
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message, ex);
				throw ex;
			}
		}

		/// <summary>
		/// Pass in the document ID to remove a training record
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public async Task<bool> RemoveRecord(string id)
		{
			try
			{
				if(string.IsNullOrEmpty(id))
				{
					return false;
				}

				List<TrainedModelCosmosDB> userQuery = _client.CreateDocumentQuery<TrainedModelCosmosDB>(
						UriFactory.CreateDocumentCollectionUri(databaseName, collectionName))
						.Where(a => a.Id == id)
						.ToList();

				foreach (var userRecord in userQuery)
				{
					await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, userRecord.Id));
				}

				return true;
			}
			catch (Exception ex)
			{
				_log.LogError(ex.Message, ex);
				throw ex;
			}
		}
	}
}
