using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Flute.Shared.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Flute.Shared.Services
{
	public class BlobStorageService : IBlobStorageService
	{
		private readonly IConfigurationReader _config;
		string storageConnectionString = string.Empty;

		static CloudStorageAccount storageAccount;
		static CloudBlobClient cloudBlobClient;
		static CloudBlobContainer cloudBlobContainer;
		public BlobStorageService(IConfigurationReader configurationReader)
		{
			_config = configurationReader;

#if DEBUG
			storageConnectionString = "UseDevelopmentStorage=true";
#else
			storageConnectionString = _config.ReadConfigurationAsync(ConfigurationReader.StorageConnectionString, true).Result;
#endif
			cloudBlobClient = storageAccount.CreateCloudBlobClient();

			 cloudBlobContainer = cloudBlobClient.GetContainerReference("ml-models");
			cloudBlobContainer.CreateIfNotExistsAsync().Wait();

			BlobContainerPermissions permissions = new BlobContainerPermissions
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			};

			cloudBlobContainer.SetPermissionsAsync(permissions).Wait();
		}

		public async Task<bool> UploadBlob(Stream stream)
		{
			try
			{
				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"Model-{DateTime.Now}.zip");
				await cloudBlockBlob.UploadFromStreamAsync(stream);

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Stream> DownloadBlob(string blobName, Stream streamTarget)
		{
			try
			{
				if(string.IsNullOrEmpty(blobName))
					return null;

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);
				await cloudBlockBlob.DownloadToStreamAsync(streamTarget);

				return streamTarget;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<List<IListBlobItem>> ListAllBlobs()
		{
			try
			{
				BlobContinuationToken blobContinuationToken = null;
				List<IListBlobItem> blobs = new List<IListBlobItem>();

				do
				{
					var results = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
					// Get the value of the continuation token returned by the listing call.
					blobContinuationToken = results.ContinuationToken;
					blobs.AddRange(results.Results);

				} while (blobContinuationToken != null);

				return blobs;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
