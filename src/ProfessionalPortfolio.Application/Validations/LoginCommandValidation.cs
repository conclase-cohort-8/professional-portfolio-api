using FluentValidation;
using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Validations
{
    public class LoginCommandValidation : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidation()
        {
            //TODO: Go to LoginCommand.cs
            // See the validation attributes used and add similar rules here.
            // See RegisterUserCommandValidator.cs for tips
            RuleFor(v => v.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            

            // TODO: remove all the attributes in the command class when you are don
        }
    }
}
