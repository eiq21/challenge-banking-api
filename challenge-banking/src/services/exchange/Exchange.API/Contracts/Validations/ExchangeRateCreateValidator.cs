using Exchange.API.Contracts.Requests;
using FluentValidation;

namespace Exchange.API.Contracts.Validations
{
    public class ExchangeRateCreateValidator: AbstractValidator<ExchangeRateCreateRequest>
    {
        public ExchangeRateCreateValidator()
        {
            RuleFor(a => a.Pair).NotEmpty().WithMessage("The Pair is Required");
            RuleFor(a => a.Offer).NotEmpty().WithMessage("The Offer is Required");
            RuleFor(a => a.Demand).NotEmpty().WithMessage("The Demand is Required");
            RuleFor(a => a.CreatedBy).NotEmpty().WithMessage("The CreatedBy is Required");

        }
    }
}
