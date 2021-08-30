using BuildingBlocks.Web.Responses;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BuildingBlocks.Web.Filters
{
    /// <summary>
    /// Asp .NET Core action filter, which validate request model
    /// </summary>
    public class ValidationFilter : IActionFilter
    {
        #region IActionFilter members

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationFailedResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        #endregion
    }
}