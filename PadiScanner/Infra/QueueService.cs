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
        _client = new(options.Value.StorageAccount.ConnectionString, options.Value.StorageAccount.QueueName);
        _logger = logger;
    }

    public async Task Enqueue(Ulid jobId)
    {
        await _client.SendMessageAsync(jobId.ToString());
        _logger.LogInformation("Job enqueued: {0}", jobId);
    }
}
