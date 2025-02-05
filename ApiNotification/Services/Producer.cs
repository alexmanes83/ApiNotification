﻿using Confluent.Kafka;
using System.Threading.Tasks;

public class KafkaProducer
{
    private readonly ProducerConfig _config;

    public KafkaProducer(string bootstrapServers)
    {
        _config = new ProducerConfig { BootstrapServers = bootstrapServers };
    }

    public async Task SendNotificationAsync(string topic, string message)
    {
        using (var producer = new ProducerBuilder<Null, string>(_config).Build())
        {
            await producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }
}