using FluentValidation;
using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Validations
{
    public class EducationCommandValidator : AbstractValidator<BaseEducationCommand>
    {
        public EducationCommandValidator()
        {
            //TODO: Go to BaseEducationCommand.cs
            // See the validation attributes used and add similar rules here.
            // See RegisterUserCommandValidator.cs for tips
            // TODO: remove all the attributes in the command class when you are done
        }
    }
}