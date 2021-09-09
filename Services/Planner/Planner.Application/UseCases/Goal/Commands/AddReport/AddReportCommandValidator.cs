using FluentValidation;
using System;

namespace Planner.Application.UseCases.Goal.Commands.AddReport
{
    public class AddReportCommandValidator : AbstractValidator<AddReportCommand>
    {
        public AddReportCommandValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);

            RuleFor(x => x.ValueOfProgress)
                .GreaterThan(0);
        }
    }
}