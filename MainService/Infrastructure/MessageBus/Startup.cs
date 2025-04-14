using System.Reflection;
using MainService.Infrastructure.MessageBus.Helpers;
using MassTransit;

namespace MainService.Infrastructure.MessageBus;

internal static class Startup
{
    internal static IServiceCollection AddMessageBus(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddDelayedMessageScheduler();
            x.SetKebabCaseEndpointNameFormatter();

            // By default, sagas are in-memory, but should be changed to a durable
            // saga repository.
            x.SetInMemorySagaRepositoryProvider();

            var entryAssembly = Assembly.GetEntryAssembly();

            x.AddActivities(entryAssembly);
            x.AddConsumers(entryAssembly);
            x.AddRequestClients();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(QueueHelper.GetRabbitMqHost());

                cfg.ReceiveEndpoint(Constants.QueueNames.Main, y =>
                {
                    y.ConfigureConsumers(context);
                    y.PrefetchCount = 1000;
                    y.ConcurrentMessageLimit = 64;
                });

                //cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    internal static void AddRequestClients(this IBusRegistrationConfigurator busRegistrationConfigurator)
    {
        //request clients to User Service
        var userQueueNameUri = new Uri(QueueHelper.GetFullQueueName(Constants.QueueNames.Streaming));
    }
}