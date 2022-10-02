using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

namespace PadiScanner.Infra.Services;

public interface IBlobStorageService
{
    Task Delete(string blobName);
    Task<Uri> Upload(string blobName, Stream stream);
}

public class BlobStorageService : IBlobStorageService
{
    private readonly PadiConfiguration _config;
    private readonly BlobServiceClient _client;

    public BlobStorageService(IOptions<PadiConfiguration> options)
    {
        _config = options.Value;

        var sharedKey = new StorageSharedKeyCredential(_config.StorageAccount.AccountName, _config.StorageAccount.AccountKey);
        _client = new(new Uri(_config.StorageAccount.BlobHost), sharedKey);
    }

    public async Task<Uri> Upload(string blobName, Stream stream)
    {
        var container = _client.GetBlobContainerClient(_config.StorageAccount.ContainerName);
        await container.UploadBlobAsync(blobName, stream);

        return new Uri(new Uri(_config.StorageAccount.BlobHost), "padi/" + blobName);
    }

    public async Task Delete(string blobName)
    {
        var container = _client.GetBlobContainerClient(_config.StorageAccount.ContainerName);
        await container.DeleteBlobIfExistsAsync(blobName);
    }

    public static string BlobNameFromUri(Uri uri)
    {
        var firstIndexOfSlash = uri.AbsolutePath.IndexOf('/');
        return uri.AbsolutePath.Substring(firstIndexOfSlash + 1);
    }
}
