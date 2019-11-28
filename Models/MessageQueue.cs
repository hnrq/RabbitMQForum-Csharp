using System;
using RabbitMQ.Client;

namespace MessagingApi.Models
{
    public class MessageQueue
    {
        public string RoutingKey { get; set; }
        public string Exchange { get; set; }
        public string QueueName { get; set; }
        public IModel Channel { get; set; }
        public MessageQueue(string exchange, string routingKey, IModel channel)
        {
          this.Exchange = exchange;
          this.RoutingKey = routingKey;
          this.Channel = channel;
          this.QueueName = channel.QueueDeclare().QueueName;
          channel.QueueBind(queue: this.QueueName, 
                            exchange: this.Exchange, 
                            routingKey: this.RoutingKey);
        }
    }

}