using FluentValidation;
using Planner.Application.Common.Validators;

namespace Planner.Application.UseCases.Planner.Commands.Create
{
    public class CreatePlannerCommandValidator : AbstractValidator<CreatePlannerCommand>
    {
        public CreatePlannerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Duration)
                .NotNull()
                .SetValidator(new DurationValidator());
        }
    }
}