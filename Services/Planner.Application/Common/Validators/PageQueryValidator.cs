using BuildingBlocks.Common.Models;
using FluentValidation;

namespace Planner.Application.Common.Validators
{
    public class PageQueryValidator : AbstractValidator<PageQuery>
    {
        public PageQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 500);
        }
    }
}