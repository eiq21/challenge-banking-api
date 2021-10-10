using FluentValidation;
using Identity.API.Contracts.Requests;

namespace Identity.API.Contracts.Validations
{
    public class LoginValidator:AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage("The Email is required");
            RuleFor(a => a.Password).NotEmpty().WithMessage("The Password is required");
        }
    }
}
