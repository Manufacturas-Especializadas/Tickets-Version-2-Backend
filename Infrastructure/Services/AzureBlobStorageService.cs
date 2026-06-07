using Application.Common.Interfaces;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Infrastructure.Services
{
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;

        private readonly string _containerName = "tickets";

        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileAsync(FileUploadDto file, CancellationToken cancellationToken)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer, cancellationToken: cancellationToken);

            var uniqueFileName = $"{Guid.NewGuid()}-{file.FileName}";
            var blobClient = containerClient.GetBlobClient(uniqueFileName);

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType };

            await blobClient.UploadAsync(
                file.Content,
                new BlobUploadOptions { HttpHeaders = blobHttpHeaders },
                cancellationToken
            );

            return blobClient.Uri.ToString();
        }
    }
}