using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Domain.Models
{
    public class Currency
    {
        public Currency(){
            IsActive = true;
            CreatedAt = DateTime.Now;
        }
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
