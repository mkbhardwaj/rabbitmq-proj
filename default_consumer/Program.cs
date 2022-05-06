using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace default_consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Consumer --");
            GetMessages();

        }

        static void GetMessages()
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queue = channel
                        .QueueDeclare(
                            queue: "SentimentAnalysis",
                            durable: false,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    channel.BasicConsume(
                        queue: "SentimentAnalysis",
                        autoAck: false,
                        consumer: consumer);
                    
                }
            }
        }
    }
}