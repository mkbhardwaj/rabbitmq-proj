using RabbitMQ.Client;
using System;
using System.Text;

namespace fanout_publisher
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("-- Producer --");
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
                    channel.ExchangeDeclare(
                        exchange: "broadcast",
                        type: ExchangeType.Fanout
                        );
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: "broadcast",
                        routingKey: "",
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("Message Sent!");
                }
            }
        }
    }
}
