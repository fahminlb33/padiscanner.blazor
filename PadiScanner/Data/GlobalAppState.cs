using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor;

namespace PadiScanner.Data;

public class GlobalAppState
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    public GlobalAppState(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    // ---- properties
    private bool isDarkMode;
    public bool IsDarkMode
    {
        get
        {
            return isDarkMode;
        }
        set
        {
            isDarkMode = value;
            _protectedLocalStorage.SetAsync(nameof(IsDarkMode), value);
            NotifyStateChanged();
        }
    }
    public string DarkModeSwitchIcon => IsDarkMode ? Icons.Outlined.ModeNight : Icons.Filled.LightMode;

    public async Task LoadFromStorage()
    {
        var isDarkModeResult = await _protectedLocalStorage.GetAsync<bool>(nameof(IsDarkMode));
        isDarkMode = isDarkModeResult.Success ? isDarkModeResult.Value : false;
    }

    // ---- events
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
}
