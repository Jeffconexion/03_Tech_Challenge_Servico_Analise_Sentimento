using Dapper;
using Service.Analise.Domain;
using Service.Analise.Repository.Contracts;
using System.Data;

namespace Service.Analise.Repository
{
    public class AnalyzeRepository : IAnalyzeRepository
    {
        private readonly IDbConnection _dbConnection;

        public AnalyzeRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<bool> Add(Analyze analyze)
        {
            var comandoSql = @"INSERT INTO Feedback (IdContact, FeedbackMessage, Sentiment, Score)
                  VALUES
                  (@IdContact, @FeedbackMessage, @Sentiment, @Score)";

            var result = await _dbConnection.ExecuteAsync(comandoSql, analyze);
            return result > 0 ? true : false;
        }
    }
}
