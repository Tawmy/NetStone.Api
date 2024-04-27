namespace NetStone.Queue;

internal static class EnvironmentVariables
{
    /// <summary>
    ///     Optional RabbitMQ Host. RabbitMQ is only active is this is set.
    /// </summary>
    public const string RabbitMqHost = "RABBITMQ_HOST";

    /// <summary>
    ///     RabbitMQ username. Mandatory if <see cref="RabbitMqHost" /> is set.
    /// </summary>
    public const string RabbitMqUsername = "RABBITMQ_USERNAME";

    /// <summary>
    ///     RabbitMQ password. Mandatory if <see cref="RabbitMqPassword" /> is set.
    /// </summary>
    public const string RabbitMqPassword = "RABBITMQ_PASSWORD";
}