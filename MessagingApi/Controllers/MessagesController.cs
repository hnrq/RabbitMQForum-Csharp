using System;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using MessagingApi.Models;

namespace MessagingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MessagesController : ControllerBase
    {
        private static Counter _COUNTER = new Counter();

        [HttpGet]
        public object GetMessage()
        {
            return new
            {
                MessagesSent = _COUNTER.Value
            };
        }

        [HttpPost]
        public ActionResult<Message> PostMessage(
            [FromServices]RabbitMQConfigurations configurations,
            [FromBody]Message message)
        {
            lock (_COUNTER)
            {
                _COUNTER.Increment();

                var factory = new ConnectionFactory()
                {
                    HostName = configurations.HostName,
                    Port = configurations.Port,
                    UserName = configurations.UserName,
                    Password = configurations.Password
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "subject_logs",
                                            type: "topic");
                    message.Id = _COUNTER.Value;
                    message.CreatedOn = DateTime.Now;
                    var serializerSettings = new JsonSerializerSettings();
                    serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message,serializerSettings));
                    channel.BasicPublish(exchange: "subject_logs",
                                            routingKey: message.Type,
                                            basicProperties: null,
                                            body: body);
                }
                return CreatedAtAction(nameof(PostMessage), new { id = _COUNTER }, message);
            
            }
        }
    }
}