using Exchange.API.Contracts.Validations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Exchange.API.Contracts.Requests
{
    public class CurrencyUpdateRequest: IValidatableObject
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
        public string UpdatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = new CurrencyUpdateValidator();
            var result = validator.Validate(this);
            return result.Errors.Select(item => new ValidationResult(item.ErrorMessage, new[] { item.PropertyName }));
        }
    }
}
