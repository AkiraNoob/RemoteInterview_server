using Microsoft.AspNetCore.Identity;

namespace Shared.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(x => x.Description).ToList();
}