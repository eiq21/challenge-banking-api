using System;

namespace Exchange.Domain.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public int ExchangeRateId { get; set; }
        public decimal FixingExchangeRate { get; set; }
        public DateTime TransactionAt { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ExchangeRate ExchangeRate { get; set; }
    }
}
