namespace Exchange.API.Contracts.Responses
{
    public class QuoteExchangeRateResponse
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public string SourceAmount { get; set; }
        public string TargetAmount { get; set; }
        public string FixingExchangeRate { get; set; }
    }
}
