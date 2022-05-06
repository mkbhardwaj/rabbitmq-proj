using RabbitMQ.Client;
using System;
using System.Text;

namespace default_producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-- Producer --");
            while (true)
            {
                Console.WriteLine("Enter 1 to enter message or 0 to exit!");
                int key = Int32.Parse(Console.ReadLine());
                if (key != 0)
                {
                    string message = Console.ReadLine();

                    Send(message);
                }
                else
                {
                    break;
                }
            }
        }

        static void Send(string message)
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
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "SentimentAnalysis",
                        basicProperties: null,
                        body: body
                        );

                    Console.WriteLine("Message Sent : {0}", message);
                }
            }
        }
    }
}
