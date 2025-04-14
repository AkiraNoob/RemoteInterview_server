using MainService.Infrastructure.MessageBus.Helpers;
using MassTransit;

namespace MainService.Infrastructure.MessageBus.Extensions;

public static class RequestClientExtensions
{
    public static Task<Response<TResponse>> GetResponseForAsync<TRequest, TResponse>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        object request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }

    public static Task<Response<TResponse>> GetResponseForAsync<TRequest, TResponse>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }

    public static Task<Response<TResponse1, TResponse2>> GetResponseForAsync<TRequest, TResponse1, TResponse2>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        object request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse1 : class
        where TResponse2 : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse1, TResponse2>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }

    public static Task<Response<TResponse1, TResponse2>> GetResponseForAsync<TRequest, TResponse1, TResponse2>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse1 : class
        where TResponse2 : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse1, TResponse2>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }

    public static Task<Response<TResponse1, TResponse2, TResponse3>> GetResponseForAsync<TRequest, TResponse1, TResponse2, TResponse3>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        object request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse1 : class
        where TResponse2 : class
        where TResponse3 : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse1, TResponse2, TResponse3>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }

    public static Task<Response<TResponse1, TResponse2, TResponse3>> GetResponseForAsync<TRequest, TResponse1, TResponse2, TResponse3>(
        this IScopedClientFactory scopedClientFactory,
        string queueName,
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : class
        where TResponse1 : class
        where TResponse2 : class
        where TResponse3 : class
    {
        var client = scopedClientFactory.CreateRequestClient<TRequest>(new Uri(QueueHelper.GetFullQueueName(queueName)));
        return client.GetResponse<TResponse1, TResponse2, TResponse3>(request, cancellationToken, TimeSpan.FromSeconds(60));
    }
}