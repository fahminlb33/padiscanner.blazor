using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace PadiScanner.Infra;

public interface IBlobStorageService
{
    Task Delete(string blobName);
    Task<Uri> Upload(string blobName, Stream stream);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly PadiConfiguration _config;
    private readonly BlobServiceClient _client;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(IOptions<PadiConfiguration> options, ILogger<BlobStorageService> logger)
    {
        _config = options.Value;
        _client = new(_config.StorageAccount.ConnectionString);
        _logger = logger;
    }

    public async Task<Uri> Upload(string blobName, Stream stream)
    {
        var container = _client.GetBlobContainerClient(_config.StorageAccount.ContainerName);
        await container.UploadBlobAsync(blobName, stream);

        _logger.LogInformation("Blob uploaded: {0}", blobName);
        return new Uri(_client.Uri, "padi/" + blobName);
    }

    public async Task Delete(string blobName)
    {
        var container = _client.GetBlobContainerClient(_config.StorageAccount.ContainerName);
        await container.DeleteBlobIfExistsAsync(blobName);

        _logger.LogInformation("Blob deleted: {0}", blobName);
    }

    public static string BlobNameFromUri(Uri uri)
    {
        var firstIndexOfSlash = uri.AbsolutePath.IndexOf('/');
        return uri.AbsolutePath.Substring(firstIndexOfSlash + 1);
    }
}
