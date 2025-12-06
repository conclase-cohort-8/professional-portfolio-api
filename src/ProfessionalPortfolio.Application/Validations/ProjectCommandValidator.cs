using FluentValidation;
using ProfessionalPortfolio.Application.Commands;

namespace ProfessionalPortfolio.Application.Validations
{
    public class ProjectCommandValidator : AbstractValidator<BaseProjectCommand>
    {
        public ProjectCommandValidator()
        {
            //TODO: Go to BaseProjectCommand.cs
            // See the validation attributes used and add similar rules here.
            // See RegisterUserCommandValidator.cs for tips
            // TODO: remove all the attributes in the command class when you are done
        }
    }
}