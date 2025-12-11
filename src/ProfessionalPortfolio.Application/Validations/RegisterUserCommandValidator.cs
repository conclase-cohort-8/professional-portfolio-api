using FluentValidation;
using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Validations
{
    internal class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(v => v.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(100).WithMessage("First Name can not exceed 100 characters");
            RuleFor(v => v.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(100).WithMessage("Last Name can not exceed 100 characters");
            RuleFor(v => v.EmailAddress)
                .NotEmpty().WithMessage("Email Address is required.")
                .EmailAddress().WithMessage("Please enter a valid email address");
            RuleFor(v => v.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
            RuleFor(v => v)
                .Must(args => IsAnAllowedEmail(args.EmailAddress))
                .WithMessage("Email address not allowed")
                .Must(args => PasswordMatches(args.Password, args.ConfirmPassword))
                .WithMessage("Password and Confirm Password must match.")
                .Must(args => IsAValidMiddleName(args.OtherName))
                .WithMessage("Other Name must be 100 charcters long or less");
        }

        private static bool IsAnAllowedEmail(string email)
        {
            var forbiddenDomains = new[] { "yahoo.com", "yahoomail.com" };
            var domain = email.Split('@').LastOrDefault();

            return !string.IsNullOrWhiteSpace(domain) ? !forbiddenDomains.Contains(domain.ToLower()) : true;
        }

        private static bool PasswordMatches(string password, string confirmPassword)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)) return false;

            return password.Equals(confirmPassword);
        }

        private static bool IsAValidMiddleName(string? otherName)
        {
            if(string.IsNullOrWhiteSpace(otherName)) return true;

            return otherName.Length <= 100;
        }
    }
}