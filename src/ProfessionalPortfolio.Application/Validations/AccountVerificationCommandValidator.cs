using FluentValidation;
using ProfessionalPortfolio.Application.Commands;
using System.Security.Cryptography.X509Certificates;

namespace ProfessionalPortfolio.Application.Validations
{
    public class AccountVerificationCommandValidator : AbstractValidator<AccountVerificationCommand>
    {
        public AccountVerificationCommandValidator()
        {
            //TODO: Go to AccountVerificationCommand.cs
            // See the validation attributes used and add similar rules here.
            // See RegisterUserCommandValidator.cs for tips

            RuleFor(v => v.Email)
               .NotEmpty().WithMessage("Email is required.")
               .EmailAddress().WithMessage("A valid email is required.");
            RuleFor(v => v.Otp)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("OTP must be 6 characters long.");


            // TODO: remove all the attributes in the command class when you are done


        }
    }
}
