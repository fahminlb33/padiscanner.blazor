namespace PadiScanner.Infra;

public class PadiConfiguration
{
    // maximum upload size is 5 MB in bytes
    public const int MaxUploadSize = 5 * 1024 * 1024;
    
    public string AnalysisServiceHost { get; set; }
    public StorageAccountConfig StorageAccount { get; set; }
}

public class StorageAccountConfig
{
    public string AccountName { get; set; }
    public string AccountKey { get; set; }
    public string ContainerName { get; set; }
    public string BlobHost => "https://" + AccountName + ".blob.core.windows.net";
}