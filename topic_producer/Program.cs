﻿using RabbitMQ.Client;
using System;
using System.Text;

namespace topic_producer
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("-- Producer --");
                Console.WriteLine("Please enter 1 to enter the message and type or enter 0 to exit!");
                int key = Int32.Parse(Console.ReadLine());
                if (key != 0)
                {
                    string message = Console.ReadLine();
                    string type = Console.ReadLine();

                    Send(message, type);
                }
                else
                {
                    break;
                }
            }
        }

        static void Send(string message, string topic)
        {
            var connectionFactory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(
                        exchange: "topic",
                        type: ExchangeType.Topic);

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(
                        exchange: "topic",
                        routingKey: topic,
                        basicProperties: null,
                        body: body);

                    Console.WriteLine("Message Sent!");
                }
            }
        }
    }
}
