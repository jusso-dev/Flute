using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Flute.Shared.Interfaces
{
	public interface IBlobStorageService
	{
		Task<bool> UploadBlob(Stream stream);
		Task<Stream> DownloadBlob(string blobName, Stream streamTarget);
		Task<List<IListBlobItem>> ListAllBlobs();
	}
}
