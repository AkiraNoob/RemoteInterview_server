using MainService.Application.Interfaces;
using MediatR;

namespace MainService.Application.Events;

public class EventPublisher(IPublisher publisher, ILogger<EventPublisher> logger) : IEventPublisher
{
    public async Task PublishAsync(INotification notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Publishing event: {EventName} at {EventTime}", notification.GetType().GetFriendlyName(), DateTime.UtcNow);

        await publisher.Publish(notification, cancellationToken);
    }
}

public static class TypeNameExtensions
{
    public static string GetFriendlyName(this Type type)
    {
        string friendlyName = type.Name;
        if (type.IsGenericType)
        {
            int iBacktick = friendlyName.IndexOf('`');
            if (iBacktick > 0)
            {
                friendlyName = friendlyName.Remove(iBacktick);
            }

            friendlyName += "<";
            Type[] typeParameters = type.GetGenericArguments();
            for (int i = 0; i < typeParameters.Length; ++i)
            {
                string typeParamName = GetFriendlyName(typeParameters[i]);
                friendlyName += (i == 0 ? typeParamName : "," + typeParamName);
            }

            friendlyName += ">";
        }

        return friendlyName;
    }
}