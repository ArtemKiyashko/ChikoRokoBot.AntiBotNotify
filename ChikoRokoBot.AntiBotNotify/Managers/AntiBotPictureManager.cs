using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using ChikoRokoBot.AntiBotNotify.Interfaces;

namespace ChikoRokoBot.AntiBotNotify.Managers
{
	public class AntiBotPictureManager : IAntiBotPictureManager
    {
        private readonly BlobContainerClient _picturesContainerClient;

        public AntiBotPictureManager(BlobContainerClient picturesContainerClient)
		{
            _picturesContainerClient = picturesContainerClient;
        }

        public async Task<IList<string>> GetPicturesUrl()
        {
            var picturesUrl = new List<string>();
            var blobs = _picturesContainerClient.GetBlobsAsync();

            await foreach (var blob in blobs)
            {
                var blobClient = _picturesContainerClient.GetBlobClient(blob.Name);
                picturesUrl.Add(blobClient.Uri.AbsoluteUri);
            }
            return picturesUrl;
        }
    }
}

