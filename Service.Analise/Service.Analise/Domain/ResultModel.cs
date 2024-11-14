using Microsoft.ML.Data;

namespace Service.Analise.Domain
{
    public class ResultModel
    {
        [ColumnName("PredictedLabel")]
        public bool PredictedSentimento { get; set; }
        public float Score { get; set; }
    }
}
