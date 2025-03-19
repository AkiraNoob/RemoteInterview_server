using Shared.Infrastructure.Identity;

namespace Shared.Infrastructure.Persistence.Extension;

public static class UserQueryExtension
{
    /// <summary>
    /// Filter the query by user identifier (email or phone number).
    /// </summary>
    /// <param name="query"></param>
    /// <param name="userIdentifier"></param>
    /// <returns></returns>
    public static IQueryable<ApplicationUser> FilterByUserIdentifier(this IQueryable<ApplicationUser> query, string userIdentifier)
    {
        return query.Where(x => x.NormalizedEmail == userIdentifier.ToUpper() || x.PhoneNumber == userIdentifier);
    }
}