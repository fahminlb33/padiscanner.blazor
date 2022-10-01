using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using PadiScanner.Data;
using System.Security.Claims;
using System.Text.Json;

namespace PadiScanner.Infra;

public class PadiAuthProvider : AuthenticationStateProvider
{
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    private const string IdentityKey = "Identity";
    private const string ExpiredAtKey = "ExpiredAt";

    public PadiAuthProvider(ProtectedLocalStorage protectedLocalStorage)
    {
        _protectedLocalStorage = protectedLocalStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // check if we already logged in
            var lastExpiredAt = await _protectedLocalStorage.GetAsync<DateTime>(ExpiredAtKey);
            if (!lastExpiredAt.Success || lastExpiredAt.Value < DateTime.Now)
            {
                return await GetAnonymous();
            }

            // set sliding expired at
            await _protectedLocalStorage.SetAsync(ExpiredAtKey, DateTime.Now.AddHours(2));

            // get current identity
            var storedPrincipal = await _protectedLocalStorage.GetAsync<string>(IdentityKey);
            if (!storedPrincipal.Success || string.IsNullOrWhiteSpace(storedPrincipal.Value))
            {
                return await GetAnonymous();
            }

            // deserialize the identity
            var user = JsonSerializer.Deserialize<User>(storedPrincipal.Value);
            return await GetClaimsForUser(user);
        }
        catch
        {
            return await GetAnonymous();
        }
    }

    public async Task LoginAsync(User user)
    {
        // save identity
        await _protectedLocalStorage.SetAsync(IdentityKey, JsonSerializer.Serialize(user));
        await _protectedLocalStorage.SetAsync(ExpiredAtKey, DateTime.Now.AddHours(2));

        // create the claims
        NotifyAuthenticationStateChanged(GetClaimsForUser(user));
    }

    public async Task LogoutAsync()
    {
        // delete local storage
        await _protectedLocalStorage.DeleteAsync(IdentityKey);
        await _protectedLocalStorage.DeleteAsync(ExpiredAtKey);

        // login as anonymous
        NotifyAuthenticationStateChanged(GetAnonymous());
    }

    private Task<AuthenticationState> GetAnonymous()
    {
        var anonymous = new ClaimsPrincipal();
        return Task.FromResult(new AuthenticationState(anonymous));
    }

    private Task<AuthenticationState> GetClaimsForUser(User user)
    {
        var identity = new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.GivenName, user.FullName),
            new (ClaimTypes.Name, user.Username),
            new (ClaimTypes.Role, user.Role.ToString()),
        }, CookieAuthenticationDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);
        return Task.FromResult(new AuthenticationState(principal));
    }
}
