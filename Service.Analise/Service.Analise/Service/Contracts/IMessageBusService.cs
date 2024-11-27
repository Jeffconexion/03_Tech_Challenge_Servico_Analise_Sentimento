namespace Service.Analise.Service.Contracts
{
    public interface IMessageBusService
    {
        Task ProcessQueue(CancellationToken cancellationToken);
    }
}
