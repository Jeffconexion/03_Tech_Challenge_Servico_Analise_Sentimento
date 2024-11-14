using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Analise.Domain;
using Service.Analise.Service.Contracts;
using System.Text;
using System.Text.Json;

namespace Service.Analise.Service
{
    public class RabbitMqService : IRabbitMqService
    {
        private const string QUEUE_SENTIMENT = "fila-sentimento";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IAnalyzeSentimentService _analyzeSentimentService;

        public RabbitMqService(IAnalyzeSentimentService analyzeSentimentService)
        {
            _analyzeSentimentService = analyzeSentimentService;

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QUEUE_SENTIMENT,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public async Task ProcessQueue(CancellationToken cancellationToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            var feedback = new Feedback();

            consumer.Received += async (sender, eventArgs) =>
            {
                var message = eventArgs.Body.ToArray();
                var feedbackMessage = Encoding.UTF8.GetString(message);

                feedback = JsonSerializer.Deserialize<Feedback>(feedbackMessage);

                await _analyzeSentimentService.ProcessAnalyze(feedback);

                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE_SENTIMENT, false, consumer);
        }
    }
}
