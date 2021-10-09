namespace Exchange.API.Contracts.Responses
{
    public class QuoteExchangeRateResponse
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal FixingExchangeRate { get; set; }
    }
}
