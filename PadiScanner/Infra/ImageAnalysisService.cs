namespace PadiScanner.Infra;

public class ImageAnalysisService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ImageAnalysisService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task Analyze(string url)
    {
        throw new NotImplementedException();
    }
}
