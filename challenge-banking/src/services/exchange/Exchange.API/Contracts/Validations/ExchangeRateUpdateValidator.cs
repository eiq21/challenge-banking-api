using Exchange.API.Contracts.Requests;
using FluentValidation;

namespace Exchange.API.Contracts.Validations
{
    public class ExchangeRateUpdateValidator: AbstractValidator<ExchangeRateUpdateRequest>
    {
        public ExchangeRateUpdateValidator()
        {
            RuleFor(a => a.Offer).NotEmpty().WithMessage("The Offer is Required");
            RuleFor(a => a.Demand).NotEmpty().WithMessage("The Demand is Required");
            RuleFor(a => a.UpdatedBy).NotEmpty().WithMessage("The UpdatedBy is Required");
        }
    }
}
