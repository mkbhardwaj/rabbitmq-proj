using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
namespace fanout_consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Consumer --");
            ReceiveMessage();
        }

        static void ReceiveMessage()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // declare the exchange
                    channel.ExchangeDeclare(
                        exchange: "broadcast",
                        type: ExchangeType.Fanout);

                    // cerate a queue
                    var queueName = channel.QueueDeclare().QueueName;

                    //bind the queue with the exchange
                    channel.QueueBind(
                        queue: queueName,
                        exchange: "broadcast",
                        routingKey: "");

                    var consumer = new EventingBasicConsumer(channel);
                    // register event handler 
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(message);
                    };

                    channel.BasicConsume(
                        queue: queueName,
                        autoAck: true,
                        consumer: consumer
                        );
                    Console.ReadLine();
                }
            }
        }
    }
}
