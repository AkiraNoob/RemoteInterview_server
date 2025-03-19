using MassTransit;
using MassTransit.Mediator;

namespace MessageBus.Extensions;

public static class MediatorExtensions
{
    public static Task<Response<TResponse>> GetResponseForAsync<TRequest, TResponse>(this IScopedMediator scopedMediator, TRequest message, CancellationToken cancellationToken)
        where TRequest : class where TResponse : class
    {
        var client = scopedMediator.CreateRequestClient<TRequest>();

        return client.GetResponse<TResponse>(message, cancellationToken);
    }
}