using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using PadiScanner.Data;
using PadiScanner.Infra;

namespace PadiScanner.Pages.Auth;

public interface IAuthViewModel
{
    Task<bool> IsAuthenticated();
    Task GuestLogin();
    Task Login(string username, string password);
    Task Logout();
}

public class AuthViewModel : IAuthViewModel
{
    private readonly PadiDataContext _context;
    private readonly PadiAuthProvider _authProvider;

    public AuthViewModel(PadiDataContext context, AuthenticationStateProvider authProvider)
    {
        _context = context;
        _authProvider = (PadiAuthProvider)authProvider;
    }

    public async Task<bool> IsAuthenticated()
    {
        var state = await _authProvider.GetAuthenticationStateAsync();
        return state!.User?.Identity?.IsAuthenticated == true;
    }

    public async Task Login(string username, string password)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Username == username)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new PadiException("Username atau password salah");
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new PadiException("Username atau password salah");
        }

        await _authProvider.LoginAsync(user);
    }

    public async Task GuestLogin()
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(x => x.Role == UserRole.Guest)
            .SingleAsync();

        await _authProvider.LoginAsync(user);
    }

    public async Task Logout()
    {
        await _authProvider.LogoutAsync();
    }
}
