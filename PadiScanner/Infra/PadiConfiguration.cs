namespace PadiScanner.Infra;

public class PadiConfiguration
{
    // maximum upload size is 10 MB in bytes
    public const int MaxUploadSize = 10 * 1024 * 1024;
    
    public StorageAccountConfig StorageAccount { get; set; }
}

public class StorageAccountConfig
{
    public string ConnectionString { get; set; }
    public string QueueName { get; set; }
    public string ContainerName { get; set; }
}