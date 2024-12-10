using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Analise.Domain;
using Service.Analise.Service.Contracts;
using System.Text;
using System.Text.Json;

namespace Service.Analise.Service
{
    public class MessageBusService : IMessageBusService
    {
        private const string QUEUE_SENTIMENT = "fila-sentimento";
        private readonly IAnalyzeSentimentService _analyzeSentimentService;
        private readonly ConnectionFactory _factory;

        public MessageBusService(IAnalyzeSentimentService analyzeSentimentService)
        {
            _analyzeSentimentService = analyzeSentimentService;

            _factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };
        }


        public async Task ProcessQueue(CancellationToken cancellationToken)
        {
            using (var connection = _factory.CreateConnection())
            {
                var channel = connection.CreateModel();
                var consumer = new EventingBasicConsumer(channel);
                var feedback = new Feedback();

                channel.QueueDeclare(
                                queue: QUEUE_SENTIMENT,
                                durable: true,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null
                             );

                consumer.Received += async (sender, eventArgs) =>
                {
                    try
                    {
                        var message = eventArgs.Body.ToArray();
                        var feedbackMessage = Encoding.UTF8.GetString(message);

                        feedback = JsonSerializer.Deserialize<Feedback>(feedbackMessage);

                        await _analyzeSentimentService.ProcessAnalyze(feedback);

                        Console.WriteLine(feedback.Name + " " + feedback.FeedbackMessage);
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                        channel.BasicNack(eventArgs.DeliveryTag, false, true);
                    }
                };

                channel.BasicConsume(QUEUE_SENTIMENT, false, consumer);

                try
                {
                    await Task.Delay(-1, cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    Console.WriteLine("Consumer stopping gracefully.");
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    channel.Close();
                }
                finally
                {
                    channel.Close();
                }
            }
        }
    }
}
