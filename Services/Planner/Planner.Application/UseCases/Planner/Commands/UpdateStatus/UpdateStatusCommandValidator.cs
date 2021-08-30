using FluentValidation;

namespace Planner.Application.UseCases.Planner.Commands.UpdateStatus
{
    public class UpdateStatusCommandValidator : AbstractValidator<UpdateStatusCommand>
    {
        public UpdateStatusCommandValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum()
                .NotEmpty();

            RuleFor(x => x.Reason)
                .NotEmpty();
        }
    }
}