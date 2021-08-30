using FluentValidation;
using Planner.Application.Common.Validators;

namespace Planner.Application.UseCases.Planner.Queries.GetByQuery
{
    public class GetPlannersQueryValidator : AbstractValidator<GetPlannersQuery>
    {
        public GetPlannersQueryValidator()
        {
            RuleFor(x => x.PageQuery)
                .SetValidator(new PageQueryValidator());

            RuleForEach(x => x.Statuses)
                .IsInEnum();
        }
    }
}