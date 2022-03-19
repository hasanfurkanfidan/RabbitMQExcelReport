using RabbitMQ.Client;
using Shared;
using System.Text;
using System.Text.Json;

namespace RabbitMQExcelReport.App.Services
{
    public class RabbitMqPublisher
    {
        private readonly RabbitMqClientService _rabbitMQClientService;
        public RabbitMqPublisher(RabbitMqClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }
        public void Publish(CreateExcelMessage createExcelMessage)
        {
            var channel = _rabbitMQClientService.Connect();
            var bodyString = JsonSerializer.Serialize(createExcelMessage);
            var bodyByte = Encoding.UTF8.GetBytes(bodyString);
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            channel.BasicPublish(exchange: RabbitMqClientService.ExchangeName, routingKey: RabbitMqClientService.RoutingExcel, basicProperties: properties, body: bodyByte);
        }
    }
}
