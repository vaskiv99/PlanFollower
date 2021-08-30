using BuildingBlocks.Common.Enums;
using BuildingBlocks.Common.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace BuildingBlocks.Web.Responses
{
    /// <summary>
    /// Represent error response
    /// </summary>
    public class ErrorResponse
    {
        public string Message { get; set; }

        public object Errors { get; set; }

        public ErrorCode ErrorCode { get; set; } = ErrorCode.Unknown;

        public ErrorResponse() { }

        public ErrorResponse(BaseException ex)
        {
            Message = ex.Message;
            Errors = ex.Errors;
            ErrorCode = ex.ErrorCode;
        }

        public ErrorResponse(ModelStateDictionary modelState)
        {
            Message = "Model validation failed";
            ErrorCode = ErrorCode.ValidationFailed;
            Errors = modelState.Keys
                .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                .ToList();
        }
    }
}