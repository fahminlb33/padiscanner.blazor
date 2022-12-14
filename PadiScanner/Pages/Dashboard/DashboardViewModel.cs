using Microsoft.EntityFrameworkCore;
using PadiScanner.Data;
using PadiScanner.Infra;

namespace PadiScanner.Pages.Dashboard;

public interface IDashboardViewModel
{
    Task<DashboardChartData> GetDiseaseChart();
    Task<DashboardChartData> GetPredictionByLocationChart();
    Task<DashboardStatsCounter> GetStatistics();
}

public class DashboardViewModel : IDashboardViewModel
{
    private readonly PadiDataContext _context;

    public DashboardViewModel(PadiDataContext context)
    {
        _context = context;
    }

    public async Task<DashboardChartData> GetDiseaseChart()
    {
        var resultsGroup = await _context.Predictions
              .AsNoTracking()
              .Where(x => x.Status == PredictionStatus.Success && x.Result != "HEALTHY")
              .GroupBy(x => x.Result)
              .Select(x => new
              {
                  Result = $"{x.Key} ({x.Count()})",
                  Count = x.Count()
              })
              .OrderByDescending(x => x.Count)
              .ToListAsync();

        return new DashboardChartData
        {
            Sizes = resultsGroup.Select(x => Convert.ToDouble(x.Count)).ToArray(),
            Labels = resultsGroup.Select(x => x.Result).ToArray(),
        };
    }

    public async Task<DashboardChartData> GetPredictionByLocationChart()
    {
        var resultsGroup = await _context.Predictions
              .AsNoTracking()
              .GroupBy(x => x.Location)
              .Select(x => new
              {
                  Location = $"{x.Key} ({x.Count()})",
                  Count = x.Count()
              })
              .OrderByDescending(x => x.Count)
              .ToListAsync();

        var top5 = resultsGroup.Take(5).ToList();
        if (resultsGroup.Count > 5)
        {
            var other = resultsGroup.Skip(5);
            top5.Add(new
            {
                Location = $"LAINNYA ({other.Sum(x => x.Count)})",
                Count = other.Sum(x => x.Count)
            });
        }

        return new DashboardChartData
        {
            Sizes = top5.Select(x => Convert.ToDouble(x.Count)).ToArray(),
            Labels = top5.Select(x => x.Location).ToArray(),
        };
    }

    public async Task<DashboardStatsCounter> GetStatistics()
    {
        var totalReport = await _context.Predictions.CountAsync();
        var negativeCount = await _context.Predictions.CountAsync(x => x.Status == PredictionStatus.Success && x.Result == "HEALTHY");
        var positiveCount = await _context.Predictions.CountAsync(x => x.Status == PredictionStatus.Success && x.Result != "HEALTHY");

        var mostInfected = await _context.Predictions
            .AsNoTracking()
            .Where(x => x.Status == PredictionStatus.Success && x.Result != "HEALTHY")
            .GroupBy(x => x.Location)
            .Select(x => new
            {
                Location = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();

        var mostReport = await _context.Predictions
            .AsNoTracking()
            .GroupBy(x => x.Location)
            .Select(x => new
            {
                Location = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count)
            .FirstOrDefaultAsync();

        return new DashboardStatsCounter
        {
            ReportCount = totalReport,
            PositiveCount = positiveCount,
            NegativeCount = negativeCount,
            MostInfectedCount = mostInfected?.Count ?? 0,
            MostInfectedLocation = mostInfected?.Location?.ToUpper() ?? "TIDAK ADA",
            MostReportCount = mostReport?.Count ?? 0,
            MostReportLocation = mostReport?.Location?.ToUpper() ?? "TIDAK ADA"
        };
    }
}
