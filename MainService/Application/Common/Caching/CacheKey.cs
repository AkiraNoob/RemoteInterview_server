namespace MainService.Application.Common.Caching;

public class CacheKey
{
    public static string UserPermission(string userId) => $"UserPermission|{userId}";
}

