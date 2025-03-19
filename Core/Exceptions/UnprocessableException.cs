using System.Net;

namespace Core.Exceptions;

public class UnprocessableException : CustomException
{
    public UnprocessableException(string message) : base(message, null, HttpStatusCode.UnprocessableContent)
    {
    }
}