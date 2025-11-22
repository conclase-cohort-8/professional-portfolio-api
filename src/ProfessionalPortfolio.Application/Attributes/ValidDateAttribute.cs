using System.ComponentModel.DataAnnotations;

namespace ProfessionalPortfolio.Application.Attributes
{
    public class ValidDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = value as DateTime?;
            if (!date.HasValue)
            {
                return ValidationResult.Success;
            }

            return date.Value > DateTime.MinValue ? 
                ValidationResult.Success :
                new ValidationResult("Invalid date");
        }
    }
}