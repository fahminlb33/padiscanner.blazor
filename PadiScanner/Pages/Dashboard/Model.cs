namespace PadiScanner.Pages.Dashboard;

public class DashboardChartData
{
    public double[] Sizes { get; set; } = new[] { 0.0 };
    public string[] Labels { get; set; } = new[] { "" };
}

public class DashboardStatsCounter
{
    public int ReportCount { get; set; }
    public int PositiveCount { get; set; }
    public int NegativeCount { get; set; }

    public string MostInfectedLocation { get; set; } = string.Empty;
    public int MostInfectedCount { get; set; }
    public string MostReportLocation { get; set; } = string.Empty;
    public int MostReportCount { get; set; }
}