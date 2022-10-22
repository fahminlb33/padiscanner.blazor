using Azure.Storage.Queues;
using Microsoft.Extensions.Options;

namespace PadiScanner.Infra;

public interface IQueueService
{
    Task Enqueue(Ulid jobId);
}

public class QueueService : IQueueService
{
    private readonly QueueClient _client;
    private readonly ILogger<QueueService> _logger;

    public QueueService(IOptions<PadiConfiguration> options, ILogger<QueueService> logger)
    {
        _logger = logger;
        
        var config = options.Value;
        _client = new(config.StorageAccount.ConnectionString, config.StorageAccount.QueueName, new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64,
        });
    }

    public async Task Enqueue(Ulid jobId)
    {
        await _client.SendMessageAsync(jobId.ToString());
        _logger.LogInformation("Job enqueued: {0}", jobId);
    }
}
