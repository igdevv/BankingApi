using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BankingApi.Exceptions
{
    public class GlobalExceptionHandler: IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            switch (exception)
            {
                case NotFoundException:
                    problemDetails.Status = StatusCodes.Status404NotFound;
                    problemDetails.Title = "Resource not found";
                    break;

                case BusinessRuleException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Business rule violation";
                    break;

                case ArgumentException:
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    problemDetails.Title = "Invalid request";
                    break;

                default:
                    problemDetails.Status = StatusCodes.Status500InternalServerError;

                    problemDetails.Title = "An unexpected error ocurred";

                    break;
            }

            problemDetails.Detail = exception.Message;

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
