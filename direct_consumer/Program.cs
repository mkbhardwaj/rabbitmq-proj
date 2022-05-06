using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace direct_consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Consumer --");
            Console.WriteLine(args.Length);
            ReceiveMessage(args);
        }

        static void ReceiveMessage(string[] types)
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                        exchange: "direct_exchange",
                        type: ExchangeType.Direct);

                    var queueName = channel.QueueDeclare().QueueName;

                    foreach (var type in types)
                    {
                        channel.QueueBind(
                            queue: queueName,
                            exchange: "direct_exchange",
                            routingKey: type);
                    }

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var type = ea.RoutingKey;
                        Console.WriteLine("message : {0}, for type : {1}", message, type);
                    };

                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: false,
                        consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }
}
