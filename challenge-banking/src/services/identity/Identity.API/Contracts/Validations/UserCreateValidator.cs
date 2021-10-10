using Identity.API.Contracts.Requests;
using FluentValidation;

namespace Identity.API.Contracts.Validations
{
    public class UserCreateValidator:AbstractValidator<UserCreateRequest>
    {
        public UserCreateValidator()
        {
            RuleFor(a => a.Email).NotEmpty().WithMessage("The Email is required");
            RuleFor(a => a.Password).NotEmpty().WithMessage("The Password is required");
        }
    }
}
