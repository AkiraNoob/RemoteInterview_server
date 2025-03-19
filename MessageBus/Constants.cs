namespace MessageBus;

public static class Constants
{
    public const string DotNetRunningInDocker = "DOTNET_RUNNING_IN_CONTAINER";

    public static class QueueNames
    {
        public const string Recruitment = "recruitment";
        public const string Streaming = "streaming";
        public const string User = "user";
        public const string Auth = "auth";
    }

    public static class RabbitMqHost
    {
        public const string DifferentNetworkRabbitMqHost = "rabbitmq://localhost";
        public const string SameNetworkRabbitMqHost = "rabbitmq";
    }
}