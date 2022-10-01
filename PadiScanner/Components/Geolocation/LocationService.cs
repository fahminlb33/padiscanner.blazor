using Microsoft.JSInterop;

namespace PadiScanner.Components.Geolocation;

public interface ILocationService
{
    Task<Location> GetLocationAsync();
}

public class LocationService : ILocationService
{
    private readonly IJSRuntime _jsRuntime;
    private static readonly Dictionary<Guid, TaskCompletionSource<Location>> _pendingRequests = new();

    public LocationService(IJSRuntime jSRuntime)
    {
        _jsRuntime = jSRuntime;
    }

    public async Task<Location> GetLocationAsync()
    {
        var requestId = Guid.NewGuid();
        var tcs = new TaskCompletionSource<Location>();

        _pendingRequests.Add(requestId, tcs);
        await _jsRuntime.InvokeAsync<Location>("PadiScannerGeolocation.GetLocation", requestId);

        return await tcs.Task;
    }

    [JSInvokable("GeolocationReceiveResponse")]
    public static void ReceiveResponse(string id, double latitude, double longitude, double accuracy)
    {
        if (!_pendingRequests.TryGetValue(Guid.Parse(id), out var tcs))
        {
            return;
        }

        tcs.SetResult(new Location
        {
            Latitude = Convert.ToDouble(latitude),
            Longitude = Convert.ToDouble(longitude),
            Accuracy = Convert.ToDouble(accuracy),
            Status = LocationStatus.Success,
            Message = "Location retrieved successfully"
        });
    }

    [JSInvokable("GeolocationReceiveErrorResponse")]
    public static void ReceiveErrorResponse(string id, int code, string message)
    {
        if (!_pendingRequests.TryGetValue(Guid.Parse(id), out var tcs))
        {
            return;
        }

        tcs.SetResult(new Location
        {
            Status = (LocationStatus)code,
            Message = code == 4 ? "Geolocation API is not supported on this browser" : message
        });
    }
}