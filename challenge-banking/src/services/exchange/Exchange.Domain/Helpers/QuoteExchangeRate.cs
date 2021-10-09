using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Domain.Helpers
{
    public class QuoteExchangeRate
    {
        public string SourceCurrency { get; set; }
        public string TargetCurrency { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal FixingExchangeRate { get; set; }
    }
}
