using FluentValidation;
using Planner.Application.Common.Validators;

namespace Planner.Application.UseCases.Goal.Commands.Update
{
    public class UpdateGoalCommandValidator : AbstractValidator<UpdateGoalCommand>
    {
        public UpdateGoalCommandValidator()
        {
            RuleFor(x => x.Duration)
                .NotNull()
                .SetValidator(new DurationValidator());

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);
        }
    }
}