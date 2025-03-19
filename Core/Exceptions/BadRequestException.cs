using System.Net;

namespace Core.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message)
        : base(message, null, HttpStatusCode.BadRequest)
    {
    }

    public BadRequestException(List<string> messages)
        : base("One or more validation errors occurred.", messages, HttpStatusCode.BadRequest)
    {
    }
}