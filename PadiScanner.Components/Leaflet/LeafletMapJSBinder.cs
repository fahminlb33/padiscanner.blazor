using Microsoft.JSInterop;

namespace PadiScanner.Components.Leaflet;

internal class LeafletMapJSBinder : IAsyncDisposable
{
    internal IJSRuntime JSRuntime;
    private Task<IJSObjectReference> _leafletMapModule;

    public LeafletMapJSBinder(IJSRuntime jsRuntime)
    {
        JSRuntime = jsRuntime;
    }

    internal async Task<IJSObjectReference> GetLeafletMapModule()
    {
        return await (_leafletMapModule ??= JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/PadiScanner.Components/leaflet-map.js").AsTask());
    }

    /// <inheritdoc/>
    public async ValueTask DisposeAsync()
    {
        if (_leafletMapModule != null)
        {
            var mapModule = await _leafletMapModule;
            await mapModule.DisposeAsync();
        }
    }
}