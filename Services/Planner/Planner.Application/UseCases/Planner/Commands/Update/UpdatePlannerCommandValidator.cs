using FluentValidation;
using Planner.Application.Common.Validators;

namespace Planner.Application.UseCases.Planner.Commands.Update
{
    public class UpdatePlannerCommandValidator : AbstractValidator<UpdatePlannerCommand>
    {
        public UpdatePlannerCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.Duration)
                .SetValidator(new DurationValidator());
        }
    }
}