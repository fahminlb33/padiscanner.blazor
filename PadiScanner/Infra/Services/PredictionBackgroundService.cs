using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PadiScanner.Data;

namespace PadiScanner.Infra.Services;

public class PredictionBackgroundService : BackgroundService
{
    private readonly PeriodicTimer _periodicTimer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<PredictionBackgroundService> _logger;
    private readonly IImageAnalysisService _imageAnalysisService;

    public PredictionBackgroundService(IServiceScopeFactory serviceScopeFactory, ILogger<PredictionBackgroundService> logger, IOptions<PadiConfiguration> options, IImageAnalysisService imageAnalysisService)
    {
        _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(options.Value.AnalysisApi.AnalysisInterval));
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
        _imageAnalysisService = imageAnalysisService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested && await _periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogDebug("Running analysis task on schedule");

            try
            {
                await RunAnalysisAsync(stoppingToken);
                _logger.LogDebug("Analysis task finished");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal exception occured in prediction worker");
            }
        }
    }

    private async Task RunAnalysisAsync(CancellationToken cancellationToken)
    {
        // create new scope to begin data context tracking
        using var scope = _serviceScopeFactory.CreateScope();

        // create data context
        using var context = scope.ServiceProvider.GetRequiredService<PadiDataContext>();

        // get all pending files to import
        var jobs = await context.Predictions
            .Where(x => x.Status == PredictionStatus.Queued || x.Status == PredictionStatus.Processing)
            .ToListAsync(cancellationToken);

        // process for each files sequentially
        // we're not using parallel since EF Core doesn't support parallel processing
        foreach (var job in jobs)
        {
            try
            {
                // throw if cancelled
                cancellationToken.ThrowIfCancellationRequested();

                // update status to processing
                job.Status = PredictionStatus.Processing;
                job.ProcessedAt = DateTime.Now;

                await context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Starting job: {0}", job.Id);

                // run prediction
                var result = await _imageAnalysisService.Analyze(new PredictionRequest
                {
                    UserId = job.UploaderId.ToString(),
                    PredictionId = job.Id.ToString(),
                    OriginalFilename = Path.GetFileName(job.OriginalImageUrl.AbsolutePath)
                });

                // update status to success
                job.Status = PredictionStatus.Success;
                job.Result = result.PredictedClass;
                job.Probabilities = result.ClassProbabilities;
                job.ProcessedAt = DateTime.Now;
                job.HeatmapImageUrl = result.Heatmap;
                job.OverlayedImageUrl = result.Superimposed;
                job.ClippedImageUrl = result.Masked;

                await context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Job finished: {0}", job.Id);
            }
            catch (OperationCanceledException)
            {
                // update status to failed
                job.Status = PredictionStatus.Failed;
                job.ProcessedAt = DateTime.Now;

                await context.SaveChangesAsync(cancellationToken);
                _logger.LogWarning("Job cancelled: {0}", job.Id);
            }
            catch (PadiException pe)
            {
                // update status to failed
                job.Status = PredictionStatus.Failed;
                job.ProcessedAt = DateTime.Now;

                await context.SaveChangesAsync(cancellationToken);
                _logger.LogError(pe, "Job failed from service: {0}", job.Id);
            }
            catch (Exception ex)
            {
                // update status to failed
                job.Status = PredictionStatus.Failed;
                job.ProcessedAt = DateTime.Now;

                await context.SaveChangesAsync(cancellationToken);
                _logger.LogError(ex, "Job failed: {0}", job.Id);
            }
        }
    }
}
