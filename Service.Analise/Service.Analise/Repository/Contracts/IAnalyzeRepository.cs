using Service.Analise.Domain;

namespace Service.Analise.Repository.Contracts
{
    public interface IAnalyzeRepository
    {
        Task<bool> Add(Analyze customerDto);
    }
}
