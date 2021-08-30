using FluentValidation;
using Planner.Domain.ValueObjects;

namespace Planner.Application.Common.Validators
{
    public class DurationValidator : AbstractValidator<Duration>
    {
        public DurationValidator()
        {
            RuleFor(x => x.Start)
                .NotEmpty();

            RuleFor(x => x.End)
                .NotEmpty();

            RuleFor(x => x.Start)
                .LessThan(x => x.End)
                .WithMessage("start date must be less than end date");
        }
    }
}