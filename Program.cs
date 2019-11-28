using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using MessagingApi.Models;

namespace MessagingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "subject_logs",  type: "topic");
                MessageQueue ftlpQueue = new MessageQueue("subject_logs", "ftlp.*", channel);
                MessageQueue damdQueue = new MessageQueue("subject_logs", "damd.*", channel);
                MessageQueue labDamdQueue = new MessageQueue("subject_logs", "labdamd.*", channel);
                channel.BasicConsume(queue: ftlpQueue.QueueName,
                                        autoAck: true,
                                        consumer: consumer.Consumer);
                channel.BasicConsume(queue: damdQueue.QueueName,
                                        autoAck: true,
                                        consumer: consumer.Consumer);
                channel.BasicConsume(queue: labDamdQueue.QueueName,
                                        autoAck: true,
                                        consumer: consumer.Consumer);
            }
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
