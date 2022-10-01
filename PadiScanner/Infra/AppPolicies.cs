using PadiScanner.Data;

namespace PadiScanner.Infra;

public static class AppPolicies
{
    public const string Administrator = nameof(UserRole.Administrator);
    public const string Member = nameof(UserRole.Member);
    public const string Guest = nameof(UserRole.Guest);
}
