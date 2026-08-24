using Application.Common;
using Application.Common.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace TradingEngine.Extensions
{
    public static class ResultExtension
    {
        public static IActionResult ToErrorResponse<T>(this ControllerBase controller, Result<T> result) where T : class
        {
            if (result.IsSuccess)
            {
                throw new Exception("Cannot convert a successful result to ProblemDetails.");
            }

            var statusCode = result.ErrorType.ToStatusCode();
            controller.HttpContext.Request.Headers.TryGetValue("X-Correlation-ID", out var correlationIdHeader);
            var correlationId = correlationIdHeader.FirstOrDefault() ?? controller.HttpContext.TraceIdentifier;

            var primaryError = result.Errors.FirstOrDefault();
            var primaryDetail = primaryError?.Message ?? "An error occurred while processing the request.";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = result.ErrorType.ToString(),
                Detail = primaryDetail,
                Instance = controller.HttpContext.Request.Path
            };

            problemDetails.Extensions["correlationId"] = correlationId;

            if (result.Errors.Count > 0)
            {
                var errorsDictionary = result.Errors
                    .GroupBy(e => e.ErrorCode)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.Message).ToArray()
                    );

                problemDetails.Extensions["errors"] = errorsDictionary;
            }

            return new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

        public static IActionResult ToSuccessResponse<T>(this ControllerBase controller, Result<T> result) where T : class
        {
            return new ObjectResult(result.Value)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        public static IActionResult ToNoContentResponse<T>(this ControllerBase controller, Result<T> result) where T : class
        {
            return new ObjectResult(null)
            {
                StatusCode = StatusCodes.Status204NoContent
            };
        }

        public static IActionResult ToCreatedResponse<T>(this ControllerBase controller, Result<T> result) where T : class
        {
            return new ObjectResult(null)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        public static int ToStatusCode(this ErrorType errorType)
        {
            return errorType switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Authentication => StatusCodes.Status401Unauthorized,
                ErrorType.Authorization => StatusCodes.Status403Forbidden,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}
