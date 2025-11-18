using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Attributes
{
    public class DisposableEmailDomainAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;
            if(string.IsNullOrEmpty(email))
            {
                return ValidationResult.Success;
            }

            var disposableEmail = new[] { "outlook.com", "yahoo.com", "yahoomail.com" };
            var domain = email.Split('@').LastOrDefault();
            if(domain != null && disposableEmail.Contains(domain.ToLower()))
            {
                return new ValidationResult("Email address is not allowed");
            }

            return ValidationResult.Success;
        }
    }
}