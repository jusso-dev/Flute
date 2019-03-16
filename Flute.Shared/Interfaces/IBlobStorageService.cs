﻿using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using static Flute.Shared.Services.BlobStorageService;

namespace Flute.Shared.Interfaces
{
	public interface IBlobStorageService
	{
		Task<bool> UploadBlob(Stream stream, TypeOfBlobUpload typeOfBlobUpload, string fileName = null);
		Task<Stream> DownloadBlob(string blobName, Stream streamTarget);
		Task<List<string>> ListBlobs(TypeOfBlobUpload typeOfBlobUpload);
	}
}
