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

                var traineDataView = context.Data.LoadFromTextFile<InputModel>(
                    path: @"C:\Users\jeffd\OneDrive\Documentos\2_Treinamentos\03_Tech_Challenge_Microsservicos\03_Tech_Challenge_Servico_Analise_Sentimento\Service.Analise\feedback_sentiment_base.csv",
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

                //PERSISTIR NO BANCO
                var resultDatabae = await _analyzeRepository.Add(analyze);
            }

        }
    }
}
