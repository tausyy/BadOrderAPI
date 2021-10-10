using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadOrder.Library.Models.Services
{
    public static class ResultErrors
    {
        public static ProblemDetails NotFound<T>(string traceId, string instance) where T : ModelBase
        {
            ProblemDetails problemDetails = new()
            {
                Type = ProblemType(StatusCodes.Status404NotFound),
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = $"The {typeof(T).Name} requested does not exist",
                Instance = instance
            };
            problemDetails.Extensions.Add(nameof(traceId), traceId);
            return problemDetails;
        }

        public static ProblemDetails Conflict(string traceId, string instance, string detail)
        {
            ProblemDetails problemDetails = new()
            {
                Type = ProblemType(StatusCodes.Status409Conflict),
                Title = nameof(Conflict),
                Status = StatusCodes.Status409Conflict,
                Detail = detail,
                Instance = instance
            };
            problemDetails.Extensions.Add(nameof(traceId), traceId);
            return problemDetails;
        }

        public static ValidationProblemDetails InvalidRole(string traceId, string role)
        {
            ValidationProblemDetails problemDetails = new()
            {
                Type = ProblemType(StatusCodes.Status400BadRequest),
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
            };
            problemDetails.Extensions.Add(nameof(traceId), traceId);
            problemDetails.Errors.Add(nameof(role), new[] { $"{role} is not a valid user Role." });
            return problemDetails;
        }

        public static ProblemDetails Unauthorized(string traceId)
        {
            ProblemDetails problemDetails = new()
            {
                Type = ProblemType(StatusCodes.Status401Unauthorized),
                Title = nameof(Unauthorized),
                Status = StatusCodes.Status401Unauthorized,
                Detail = "Not authorized to access this resource"
            };
            problemDetails.Extensions.Add(nameof(traceId), traceId);
            return problemDetails;
        }


        private static string ProblemType(int statusCode) => statusCode switch
        {
            StatusCodes.Status400BadRequest => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            StatusCodes.Status401Unauthorized => "https://tools.ietf.org/html/rfc7235#section-3.1",
            StatusCodes.Status404NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            StatusCodes.Status409Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
            StatusCodes.Status500InternalServerError => "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            _ => null
        };
    }
}
