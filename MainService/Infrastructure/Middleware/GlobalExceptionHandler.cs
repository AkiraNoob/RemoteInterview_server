using MainService.Application.Exceptions;
using MainService.Application.Middleware;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace MainService.Infrastructure.Middleware;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var errorResult = new ErrorResult
        {
            Source = exception.TargetSite?.DeclaringType?.FullName,
            Messages = [exception.Message]
        };

        if (exception is not CustomException && exception.InnerException != null)
        {
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
        }

        switch (exception)
        {
            case CustomException e:
                httpContext.Response.StatusCode = errorResult.StatusCode = (int)e.StatusCode;
                if (e.ErrorMessages is not null)
                {
                    errorResult.Messages = e.ErrorMessages;
                }

                break;

            case KeyNotFoundException:
                httpContext.Response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.NotFound;
                break;

            default:
                httpContext.Response.StatusCode = errorResult.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
        }

        await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken: cancellationToken);

        return true;
    }
}