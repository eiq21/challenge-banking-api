using Exchange.API.Contracts.Requests;
using FluentValidation;

namespace Exchange.API.Contracts.Validations
{
    public class CurrencyUpdateValidator: AbstractValidator<CurrencyUpdateRequest>
    {
        public CurrencyUpdateValidator()
        {
            RuleFor(a => a.Name).NotEmpty().WithMessage("The Name is Required");
            RuleFor(a => a.Code).NotEmpty().WithMessage("The Code is Required");
            RuleFor(a => a.Symbol).NotEmpty().WithMessage("The Symbol is Required");
            RuleFor(a => a.UpdatedBy).NotEmpty().WithMessage("The UpdatedBy is Required");

        }
    }
}
