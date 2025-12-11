using FluentValidation;
using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Validations
{
    public class ExperienceCommandValidator : AbstractValidator<BaseExperienceCommand>
    {
        public ExperienceCommandValidator()
        {
            //TODO: Go to BaseExperienceCommand.cs
            // See the validation attributes used and add similar rules here.
            // See RegisterUserCommandValidator.cs for tips
            // TODO: remove all the attributes in the command class when you are done
        }
    }
}