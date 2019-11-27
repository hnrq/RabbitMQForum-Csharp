
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Backend.Models;

namespace Backend
{
  class Program
  {
    private static IConfiguration _configuration;

    static void Main(string[] args)
    {
      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile($"appsettings.json");
      _configuration = builder.Build();
      var rabbitMQConfigurations = new RabbitMQConfigurations();
      new ConfigureFromConfigurationOptions<RabbitMQConfigurations>(
        _configuration.GetSection("RabbitMQConfigurations"))
          .Configure(rabbitMQConfigurations);

      var factory = new ConnectionFactory()
      {
        HostName = rabbitMQConfigurations.HostName,
        Port = rabbitMQConfigurations.Port,
        UserName = rabbitMQConfigurations.UserName,
        Password = rabbitMQConfigurations.Password
      };

      using (var connection = factory.CreateConnection())
      using (var channel = connection.CreateModel())
      {
          channel.ExchangeDeclare(exchange: "subject_logs",  type: "topic");
          MessageQueue ftlpQueue = new MessageQueue("subject_logs", "ftlp.*", channel);
          MessageQueue damdQueue = new MessageQueue("subject_logs", "damd.*", channel);
          MessageQueue labDamdQueue = new MessageQueue("subject_logs", "labdamd.*", channel);

          BasicConsumer consumer = new BasicConsumer("subject_logs", channel);
          
          Console.WriteLine("Waiting for messages... CTRL+C to exit.");
          
          channel.BasicConsume(queue: ftlpQueue.QueueName,
                                 autoAck: true,
                                 consumer: consumer.Consumer);
          channel.BasicConsume(queue: damdQueue.QueueName,
                                 autoAck: true,
                                 consumer: consumer.Consumer);
          channel.BasicConsume(queue: labDamdQueue.QueueName,
                                 autoAck: true,
                                 consumer: consumer.Consumer);

          Console.WriteLine(" Press [enter] to exit.");
          Console.ReadLine();
      }
    }
  }
}