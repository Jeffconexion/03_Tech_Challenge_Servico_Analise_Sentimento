using Service.Analise.Service.Contracts;

namespace Service.Analise
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMessageBusService _rabbitMqService;


        public Worker(ILogger<Worker> logger, IMessageBusService rabbitMqService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _rabbitMqService.ProcessQueue(stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
