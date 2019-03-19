using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
			CloudStorageAccount.TryParse(storageConnectionString, out storageAccount);

			cloudBlobClient = storageAccount.CreateCloudBlobClient();

			cloudBlobContainer = cloudBlobClient.GetContainerReference("ml-models");
			cloudBlobContainer.CreateIfNotExistsAsync().Wait();

			BlobContainerPermissions permissions = new BlobContainerPermissions
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			};

			cloudBlobContainer.SetPermissionsAsync(permissions).Wait();
		}

		/// <summary>
		/// Determine naming convention for blob to be uploaded
		/// </summary>
		public enum TypeOfBlobUpload
		{
			TrainingFile,
			ModelFile
		}

		public async Task<bool> UploadBlob(Stream stream, TypeOfBlobUpload typeOfBlobUpload, string usersEmail, string fileName = null)
		{
			try
			{
				if(typeOfBlobUpload == TypeOfBlobUpload.ModelFile)
				{
					CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"Models/{usersEmail}/Model-{Guid.NewGuid()}.zip");
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}
				else
				{
					CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"Models/{usersEmail}/{fileName}");
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<Stream> DownloadBlob(string usersEmail, string blobName, Stream streamTarget)
		{
			try
			{
				if(string.IsNullOrEmpty(blobName) || string.IsNullOrEmpty(usersEmail))
					return null;

				CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"Models/{usersEmail}/{blobName}");
				await cloudBlockBlob.DownloadToStreamAsync(streamTarget);

				return streamTarget;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public async Task<List<string>> ListBlobs(TypeOfBlobUpload typeOfBlobUpload, string usersEmail)
		{
			try
			{
				if(string.IsNullOrEmpty(usersEmail))
				{
					return null;
				}

				BlobContinuationToken continuationToken = null;

				var directory = cloudBlobContainer.GetDirectoryReference($"Models/{usersEmail}");
				var blobResultSegment = await directory.ListBlobsSegmentedAsync(continuationToken);

				List<string> blobNames = new List<string>();

				if(typeOfBlobUpload == TypeOfBlobUpload.TrainingFile)
				{
					// Select blobs by name, where the name suggests the file is the training file
					blobNames.AddRange(blobResultSegment.Results
						.OfType<CloudBlockBlob>()
						.Where(a => a.Uri.Segments.Last().Contains(".csv"))
						.OrderByDescending(b => b.Properties.LastModified)
						.Select(i => i.Uri.Segments.Last()).ToList());
				}
				else
				{
					// Select blobs by name, where the name suggests the file is the model file
					blobNames.AddRange(blobResultSegment.Results
						.OfType<CloudBlockBlob>()
						.Where(a => a.Uri.Segments.Last().Contains(".zip"))
						.OrderByDescending(b => b.Properties.LastModified)
						.Select(i => i.Uri.Segments.Last()).ToList());
				}

				return blobNames;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Scrub blob storage of any instance of the training file
		/// </summary>
		/// <param name="usersEmail"></param>
		/// <returns></returns>
		public async Task<bool> RemoveTrainingFile(string usersEmail)
		{
			try
			{
				var listOfTrainingFiles = await this.ListBlobs(TypeOfBlobUpload.TrainingFile, usersEmail);

				foreach(var item in listOfTrainingFiles)
				{
					CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(item);
					await cloudBlockBlob.DeleteIfExistsAsync();
				}

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
