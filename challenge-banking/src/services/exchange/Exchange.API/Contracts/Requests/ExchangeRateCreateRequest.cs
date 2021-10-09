using Exchange.API.Contracts.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exchange.API.Contracts.Requests
{
    public class ExchangeRateCreateRequest: IValidatableObject
    {
        public string Pair { get; set; }
        public decimal Offer { get; set; }
        public decimal Demand { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new ExchangeRateCreateValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
