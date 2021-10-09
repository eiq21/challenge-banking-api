using Exchange.API.Contracts.Requests;
using FluentValidation;

namespace Exchange.API.Contracts.Validations
{
    public class CurrencyCreateValidator:AbstractValidator<CurrencyCreateRequest>
    {
        public CurrencyCreateValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("The Name is required");
            RuleFor(a => a.Symbol).NotEmpty().WithMessage("The Symbol is required");
            RuleFor(a => a.Code).NotEmpty().WithMessage("The Code is required");
        }
    }
}
