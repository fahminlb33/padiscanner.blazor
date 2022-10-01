using Microsoft.JSInterop;

namespace PadiScanner.Components.Leaflet;

internal class LeafletMapJSBinder : IAsyncDisposable
{
    internal IJSRuntime JSRuntime;
    private IJSObjectReference _leafletMapModule;

    public LeafletMapJSBinder(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
    }

    internal async Task<IJSObjectReference> GetLeafletMapModule()
    {
        _leafletMapModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/leaflet-map.js");
        return _leafletMapModule;
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_leafletMapModule != null)
        {
            await _leafletMapModule.DisposeAsync();
        }
    }
}