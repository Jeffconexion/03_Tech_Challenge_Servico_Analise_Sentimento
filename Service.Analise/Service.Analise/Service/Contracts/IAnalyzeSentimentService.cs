using Service.Analise.Domain;

namespace Service.Analise.Service.Contracts
{
    public interface IAnalyzeSentimentService
    {
        Task ProcessAnalyze(Feedback messageNotification);
    }
}
