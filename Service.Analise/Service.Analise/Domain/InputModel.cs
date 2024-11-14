using Microsoft.ML.Data;

namespace Service.Analise.Domain
{
    public class InputModel
    {
        [LoadColumn(0)]
        public bool Sentimento { get; set; }

        [LoadColumn(1)]
        public string SentimentoTexto { get; set; }
    }
}
