using Microsoft.EntityFrameworkCore;
using PadiScanner.Infra;
using System.Text;

namespace PadiScanner.Pages.Maps;

public record MarkerData
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Title { get; set; }
    public string PopupContent { get; set; }
}

public interface IMapsViewModel
{
    IAsyncEnumerable<MarkerData> GetMapMarkers();
}

public class MapsViewModel : IMapsViewModel
{
    private readonly PadiDataContext _context;

    public MapsViewModel(PadiDataContext context)
    {
        _context = context;
    }

    public async IAsyncEnumerable<MarkerData> GetMapMarkers()
    {
        var detections = await _context.Predictions
            .GroupBy(x => new { x.Latitude, x.Longitude })
            .Select(x=> new
            {
                Coord = x.Key,
                Results = x.ToList()
            })
            .ToListAsync();

        foreach (var detection in detections)
        {
            var locations = string.Join(", ", detection.Results.Select(x => x.Location).Distinct());
            var reportTotal = detection.Results.Count();
            var reportByResult = new StringBuilder();
            foreach (var resultStatus in detection.Results.GroupBy(x => x.Result))
            {
                var key = string.IsNullOrWhiteSpace(resultStatus.Key) ? "MENUNGGU" : resultStatus.Key;
                reportByResult.AppendFormat("{0}: {1}<br />", key, resultStatus.Count());
            }
            
            yield return new MarkerData
            {
                Latitude = detection.Coord.Latitude,
                Longitude = detection.Coord.Longitude,
                Title = "Laporan Prediksi",
                PopupContent = $"Lokasi: {locations}<br />Total laporan: {reportTotal}<br /><b>Hasil prediksi:</b><br />{reportByResult}"
            };
        }
    }
}
