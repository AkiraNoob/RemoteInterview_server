using Microsoft.AspNetCore.Identity;

namespace MainService.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(x => x.Description).ToList();
}