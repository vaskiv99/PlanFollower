using FluentValidation;
using Planner.Application.Common.Validators;
using Planner.Domain.AggregatesModel.GoalAggregate.Enums;

namespace Planner.Application.UseCases.Goal.Commands.Create
{
    public class CreateGoalCommandValidator : AbstractValidator<CreateGoalCommand>
    {
        public CreateGoalCommandValidator()
        {
            RuleFor(x => x.Duration)
                .NotNull()
                .SetValidator(new DurationValidator());

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.TrackingType)
                .IsInEnum();

            RuleFor(x => x.EqualType)
                .IsInEnum();

            RuleFor(x => x.AbstractGoalValue)
                .GreaterThan(0);

            RuleFor(x => x.AbstractGoalValue)
                .Must(x => x.HasValue)
                .When(x => x.TrackingType == TrackingType.AbstractGoalValue)
                .WithMessage("Abstract goal value is required");

            RuleFor(x => x.Frequency)
                .NotNull()
                .SetValidator(new FrequencyValidator());
        }
    }
}