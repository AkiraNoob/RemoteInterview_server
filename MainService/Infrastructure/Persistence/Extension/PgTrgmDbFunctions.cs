using Microsoft.EntityFrameworkCore;

namespace MainService.Infrastructure.Persistence.Extension;

public static class PgTrgmDbFunctions
{
    [DbFunction("similarity", IsBuiltIn = true)]
    public static double Similarity(string source, string target)
        => throw new NotImplementedException(); // never called directly
}