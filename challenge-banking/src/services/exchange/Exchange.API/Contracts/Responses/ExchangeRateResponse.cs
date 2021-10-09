using System;

namespace Exchange.API.Contracts.Responses
{
    public class ExchangeRateResponse
    {
        public int Id { get; set; }
        public string Pair { get; set; }
        public string Offer { get; set; }
        public string Demand { get; set; }
        public DateTime ExchangeRateAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
