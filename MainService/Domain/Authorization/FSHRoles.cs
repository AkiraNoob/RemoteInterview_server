using System.Collections.ObjectModel;

namespace MainService.Domain.Authorization;

public static class FSHRoles
{
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);
    public const string Company = nameof(Company);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(new[]
    {
        Admin,
        Company,
        Basic
    });
}