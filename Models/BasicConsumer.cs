using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Backend.Models
{
    public class BasicConsumer
    {
        public EventingBasicConsumer Consumer { get; set; } 
        public string QueueName { get; set; }
        public BasicConsumer(string exchange, IModel channel)
        {
          this.Consumer = new EventingBasicConsumer(channel);
          this.Consumer.Received += (model, ea) => 
          {
            string message = Encoding.UTF8.GetString(ea.Body);
            Console.WriteLine(" [x] Received '{0}':'{1}'",
                                  ea.RoutingKey,
                                  message);
          };
        }
    }

}