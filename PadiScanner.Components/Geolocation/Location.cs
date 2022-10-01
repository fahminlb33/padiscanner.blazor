namespace PadiScanner.Components.Geolocation;
public record Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Accuracy { get; set; }

    public LocationStatus Status { get; set; } = LocationStatus.Success;
    public string Message { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"Location: ({Latitude}, {Longitude}) with accuracy {Accuracy}";
    }
}

public enum LocationStatus
{
    Success,
    PermissionDenied,
    PositionUnavailable,
    Timeout,
    Unknown
}