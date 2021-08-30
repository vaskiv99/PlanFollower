using BuildingBlocks.Common.Exceptions;
using BuildingBlocks.Web.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace BuildingBlocks.Web.Filters
{
    /// <summary>
    /// Asp .NET Core exception filter, which catch unhandled exception.
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        #region Private fields

        private readonly ILogger<GlobalExceptionFilter> _logger;

        #endregion

        #region Constructors

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        #endregion

        #region IExceptionFilter members

        public void OnException(ExceptionContext context)
        {
            var (error, statusCode) = PrepareResponseForException(context.Exception);

            context.ExceptionHandled = true;

            context.Result = new ObjectResult(error)
            {
                StatusCode = statusCode
            };
        }

        #endregion

        #region Private methods

        private (ErrorResponse, int) PrepareResponseForException(Exception exception)
        {
            ErrorResponse error;
            int statusCode;

            switch (exception)
            {
                case BaseException dataEx:
                    statusCode = dataEx.StatusCode;
                    error = new ErrorResponse(dataEx);
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    error = new ErrorResponse
                    {
                        Message = "Internal Server Error"
                    };

                    _logger?.LogCritical(exception, $"REST API Internal Server Error: {exception.Message}");

                    break;
            }

            return (error, statusCode);
        }

        #endregion
    }
}