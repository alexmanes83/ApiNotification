using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

public class NotificationConsumer
{
    private readonly string _topic;
    private readonly ConsumerConfig _config;

    public NotificationConsumer(string topic, string bootstrapServers)
    {
        _topic = topic;
        _config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = "notification-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    public void StartConsuming(CancellationToken cancellationToken)
    {
        using (var consumer = new ConsumerBuilder<Ignore, string>(_config).Build())
        {
            consumer.Subscribe(_topic);
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var cr = consumer.Consume(cancellationToken);
                    Console.WriteLine($"Mensagem recebida do tópico {_topic}: {cr.Value}");
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Erro ao consumir mensagem: {e.Error.Reason}");
                }
            }
        }
    }
}