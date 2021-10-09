using System;
using System.Collections.Generic;

namespace Exchange.Domain.Models
{
    public class ExchangeRate
    {
       public ExchangeRate()
        {
            IsActive = true;
            CreatedAt = DateTime.Now;
            ExchangeRateAt = DateTime.Now;
            Transaction = new HashSet<Transaction>();
        }
        public int ExchangeRateId { get; set; }
        public string Pair { get; set; }
        public decimal Offer { get; set; }
        public decimal Demand { get; set; }
        public DateTime ExchangeRateAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}
