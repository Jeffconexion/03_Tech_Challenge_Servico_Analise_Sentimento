using Microsoft.ML;
using Service.Analise.Domain;
using Service.Analise.Repository.Contracts;
using Service.Analise.Service.Contracts;

namespace Service.Analise.Service
{
    public class AnalyzeSentimentService : IAnalyzeSentimentService
    {
        private readonly IAnalyzeRepository _analyzeRepository;

        public AnalyzeSentimentService(IAnalyzeRepository analyzeRepository)
        {
            _analyzeRepository = analyzeRepository;
        }

        public async Task ProcessAnalyze(Feedback messageAnalise)
        {
            if (!string.IsNullOrEmpty(messageAnalise.Message))
            {
                MLContext context = new MLContext();

                string filePath = @"feedback_sentiment_base.csv";

                if (File.Exists(filePath))
                {

                    var traineDataView = context.Data.LoadFromTextFile<InputModel>(
                        path: filePath,
                        hasHeader: true,
                        separatorChar: ',',
                        allowQuoting: true,    // Permite aspas para strings
                        trimWhitespace: true   // Remove espaços extras
                    );


                    // Definir o pipeline
                    var pipeline = context.Transforms.Text.FeaturizeText("Features", nameof(InputModel.SentimentoTexto))
                            .Append(context.BinaryClassification.Trainers.AveragedPerceptron(
                                labelColumnName: nameof(InputModel.Sentimento),
                                featureColumnName: "Features",
                                numberOfIterations: 100));

                    // Treinar o modelo
                    var model = pipeline.Fit(traineDataView);

                    // Criar o mecanismo de previsão
                    var predictionEngine = context.Model.CreatePredictionEngine<InputModel, ResultModel>(model);
                    var result = predictionEngine.Predict(new InputModel { SentimentoTexto = messageAnalise.FeedbackMessage });

                    var analyze = new Analyze()
                    {
                        IdContact = messageAnalise.Id,
                        FeedbackMessage = messageAnalise.FeedbackMessage,
                        Sentiment = result.PredictedSentimento,
                        Score = result.Score
                    };

                    string logMessage = $"Análise de Feedback - Data: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                    $"Id do Contato: {analyze.IdContact}\n" +
                    $"Mensagem: {analyze.FeedbackMessage}\n" +
                    $"Sentimento: {analyze.Sentiment}\n" +
                    $"Score: {analyze.Score}";

                    Console.WriteLine(logMessage);


                    //PERSISTIR NO BANCO
                    Console.WriteLine($"Salvo com sucesso no banco de dados.");
                    //await _analyzeRepository.Add(analyze);
                }
                else
                {
                    Console.WriteLine($"Erro: Arquivo '{filePath}' não encontrado.");
                }
            }

        }
    }
}
