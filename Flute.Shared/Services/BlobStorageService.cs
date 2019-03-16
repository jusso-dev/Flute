﻿using System;
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

		public async Task<bool> UploadBlob(Stream stream, TypeOfBlobUpload typeOfBlobUpload, string fileName = null)
		{
			try
			{
				if(typeOfBlobUpload == TypeOfBlobUpload.ModelFile)
				{
					CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference($"Model-{Guid.NewGuid()}.zip");
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}
				else
				{
					CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
					await cloudBlockBlob.UploadFromStreamAsync(stream);
				}

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

		public async Task<List<string>> ListBlobs(TypeOfBlobUpload typeOfBlobUpload)
		{
			try
			{
				BlobContinuationToken continuationToken = null;

				var blobResultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(continuationToken);

				List<string> blobNames = new List<string>();

				if(typeOfBlobUpload == TypeOfBlobUpload.TrainingFile)
				{
					// Select blobs by name, where the name suggests the file is the training file
					blobNames.AddRange(blobResultSegment.Results
						.Where(a => a.Uri.Segments.Last().Contains(".csv"))
						.Select(i => i.Uri.Segments.Last()).ToList());
				}
				else
				{
					// Select blobs by name, where the name suggests the file is the model file
					blobNames.AddRange(blobResultSegment.Results
						.Where(a => a.Uri.Segments.Last().Contains(".zip"))
						.Select(i => i.Uri.Segments.Last()).ToList());
				}

				return blobNames;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
