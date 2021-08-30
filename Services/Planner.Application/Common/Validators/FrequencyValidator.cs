using FluentValidation;
using Planner.Domain.AggregatesModel.GoalAggregate.ValueObjects;

namespace Planner.Application.Common.Validators
{
    public class FrequencyValidator : AbstractValidator<Frequency>
    {
        public FrequencyValidator()
        {
            RuleFor(x => x.TimePeriod)
                .IsInEnum();

            RuleFor(x => x.CountOfTimes)
                .GreaterThan(0);

            RuleFor(x => x.CountOfPeriods)
                .GreaterThan(0);
        }
    }
}