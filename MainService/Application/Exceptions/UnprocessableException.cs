using System.Net;

namespace MainService.Application.Exceptions;

public class UnprocessableException : CustomException
{
    public UnprocessableException(string message) : base(message, null, HttpStatusCode.UnprocessableContent)
    {
    }
}