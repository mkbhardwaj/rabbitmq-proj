using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace topic_consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Consumer --");
            ReceiveMessage(args);
        }

        static void ReceiveMessage(string[] topics)
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                        exchange: "topic",
                        type: ExchangeType.Topic);
                    var queueName = channel.QueueDeclare().QueueName;

                    foreach (var topic in topics)
                    {
                        channel.QueueBind(
                            queue: queueName,
                            exchange: "topic",
                            routingKey: topic,
                            arguments: null
                            );
                    }

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: true,
                        consumer: consumer);
                    Console.Read();
                }
            }
        }
    }
}
