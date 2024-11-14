namespace Service.Analise.Service.Contracts
{
    public interface IRabbitMqService
    {
        Task ProcessQueue(CancellationToken cancellationToken);
    }
}
