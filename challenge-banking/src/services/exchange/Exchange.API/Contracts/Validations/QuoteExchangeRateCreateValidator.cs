using Exchange.API.Contracts.Requests;
using FluentValidation;

namespace Exchange.API.Contracts.Validations
{
    public class QuoteExchangeRateCreateValidator: AbstractValidator<QuoteExchangeRateCreateRequest>
    {
        public QuoteExchangeRateCreateValidator()
        {
            RuleFor(a => a.Amount).NotEmpty().WithMessage("The Amount is Required");
            RuleFor(a => a.SourceCurrency).NotEmpty().WithMessage("The SourceCurrency is Required");
            RuleFor(a => a.TargetCurrency).NotEmpty().WithMessage("The TargetCurrency is Required");
        }
    }
}
