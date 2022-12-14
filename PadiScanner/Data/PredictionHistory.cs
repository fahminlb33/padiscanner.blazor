using System.ComponentModel.DataAnnotations;

namespace PadiScanner.Data;

public class PredictionHistory
{
    [Key]
    public Ulid Id { get; set; }

    public DateTime UploadedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    [MaxLength(255)]
    public string Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Uri OriginalImageUrl { get; set; }
    public Uri? HeatmapImageUrl { get; set; }
    public Uri? OverlayedImageUrl { get; set; }
    public Uri? ClippedImageUrl { get; set; }
    [MaxLength(100)]
    public string? Result { get; set; }
    public PredictionStatus Status { get; set; }
    public Dictionary<string, double>? Probabilities { get; set; } = new Dictionary<string, double>();
    public double Severity { get; set; }

    [StringLength(26)]
    public Ulid UploaderId { get; set; }
    public User Uploader { get; set; }
}
