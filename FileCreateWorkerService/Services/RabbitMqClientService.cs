using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace FileCreateWorkerService.Services
{
    public class RabbitMqClientService
    {
        private readonly ConnectionFactory _connectionFactory;
        private IConnection _connection;
        private IModel _channel;
        public static string ExchangeName = "ExcelDirectExchange";
        public static string RoutingExcel = "excel-route";
        public static string QueueName = "queue-excel-image";
        private readonly ILogger<RabbitMqClientService> _logger;
        public RabbitMqClientService(ConnectionFactory connectionFactory, ILogger<RabbitMqClientService> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }
        public IModel Connect()
        {
            _connection = _connectionFactory.CreateConnection();
            if (_channel is { IsOpen: true })
                return _channel;

            _channel = _connection.CreateModel();

            _logger.LogInformation("Rabbit MQ ile bağlantı oluşturuldu");
            return _channel;
        }

        public void Dispose()
        {
            _channel?.Close();
            _channel?.Dispose();
            _channel = null;
            _connection?.Close();
            _connection?.Dispose();
            _logger.LogInformation("Rabbit Mq ile bağlantı koptu");
        }
    }
}
