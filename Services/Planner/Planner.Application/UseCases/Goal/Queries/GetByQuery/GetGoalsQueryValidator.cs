using FluentValidation;
using Planner.Application.Common.Validators;

namespace Planner.Application.UseCases.Goal.Queries.GetByQuery
{
    public class GetGoalsQueryValidator : AbstractValidator<GetGoalsQuery>
    {
        public GetGoalsQueryValidator()
        {
            RuleFor(x => x.PageQuery)
                .SetValidator(new PageQueryValidator());
        }
    }
}