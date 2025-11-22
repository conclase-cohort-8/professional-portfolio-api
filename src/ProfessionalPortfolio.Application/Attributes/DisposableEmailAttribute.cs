using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Attributes
{
    public class DisposableEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var email = value as string;
            if(email == null)
            {
                return ValidationResult.Success;
            }

            var disposableDomains = new[] { "outlook.com", "yahoo.com", "yahoomail.com" };
            var domain = email.Split('@').LastOrDefault();
            if(domain != null && disposableDomains.Contains(domain.ToLower()))
            {
                return new ValidationResult("Email address not allowed");
            }

            return ValidationResult.Success;
        }
    }
}