﻿namespace PadiScanner.Infra;

public class PadiConfiguration
{
    // maximum upload size is 5 MB in bytes
    public const int MaxUploadSize = 5 * 1024 * 1024;

    public AnalysisApiConfig AnalysisApi { get; set; }
    public StorageAccountConfig StorageAccount { get; set; }
}

public class AnalysisApiConfig
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int AnalysisInterval { get; set; }
}

public class StorageAccountConfig
{
    public string AccountName { get; set; }
    public string AccountKey { get; set; }
    public string ContainerName { get; set; }
    public string BlobHost => "https://" + AccountName + ".blob.core.windows.net";
}