using MainService.Application.Slices.RecruitmentSlice.DTOs;
using MainService.Domain.Models;

namespace MainService.Application.Helpers;

public static class AutoUpdateFields
{
    public static void Update<T, K>(this K result, T updateSource) where K : class
    {
        if (result == null || updateSource == null) return;

        var requestProperties = typeof(T).GetProperties();
        var resultProperties = typeof(K).GetProperties().ToDictionary(p => p.Name, p => p);

        foreach (var prop in requestProperties)
        {
            if (resultProperties.TryGetValue(prop.Name, out var resultProp) && resultProp.CanWrite)
            {
                var newValue = prop.GetValue(updateSource);
                if (newValue != null) // Only update non-null values
                {
                    resultProp.SetValue(result, newValue);
                }
            }
        }
    }
}
