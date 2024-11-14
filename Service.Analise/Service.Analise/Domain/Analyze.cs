namespace Service.Analise.Domain
{
    public class Analyze
    {
        public string? IdContact { get; set; }
        public string? FeedbackMessage { get; set; }
        public bool Sentiment { get; set; }
        public float Score { get; set; }
    }
}