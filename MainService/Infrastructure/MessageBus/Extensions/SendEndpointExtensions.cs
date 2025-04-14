using MainService.Infrastructure.MessageBus.Helpers;
using MassTransit;

namespace MainService.Infrastructure.MessageBus.Extensions;

public static class SendEndpointExtensions
{
    public static async Task SendAsync<TMessage>(this ISendEndpointProvider sendEndpointProvider, string queueName, TMessage message, CancellationToken cancellationToken = default)
        where TMessage : class
    {
        var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri(QueueHelper.GetFullQueueName(queueName)));
        await endpoint.Send(message, cancellationToken);
    }
}