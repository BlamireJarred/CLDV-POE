// Services/AzureStorageService.cs
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace POEPart1_VenueBookingSystem.Services
{
    public class AzureStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string ContainerName = "venue-images";

        public AzureStorageService(string connectionString)
        {
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }
    }
}