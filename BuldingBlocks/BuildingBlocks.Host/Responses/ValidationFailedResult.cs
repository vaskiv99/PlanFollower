using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BuildingBlocks.Web.Responses
{
    /// <summary>
    /// Http result, which represent, if validation failed.
    /// </summary>
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ErrorResponse(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}