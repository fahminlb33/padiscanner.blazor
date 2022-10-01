using Microsoft.JSInterop;

namespace PadiScanner.Components.Storage;

public interface ILocalStorageService
{
    Task<string?> GetStringAsync(string key);
    Task RemoveAsync(string key);
    Task SaveStringAsync(string key, string value);
}

public class LocalStorageService : ILocalStorageService
{
    private readonly IJSRuntime jsruntime;

    public LocalStorageService(IJSRuntime jsruntime)
    {
        this.jsruntime = jsruntime;
    }

    public async Task RemoveAsync(string key)
    {
        await jsruntime.InvokeVoidAsync("localStorage.removeItem", key).ConfigureAwait(false);
    }

    public async Task SaveStringAsync(string key, string value)
    {
        await jsruntime.InvokeVoidAsync("localStorage.setItem", key, value).ConfigureAwait(false);
    }

    public async Task<string?> GetStringAsync(string key)
    {
        return await jsruntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
    }
}
