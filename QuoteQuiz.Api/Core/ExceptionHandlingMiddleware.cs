using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Application.Common;
using Newtonsoft.Json.Serialization;

namespace QuoteQuiz.Api.Core
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (QuoteQuizApplicationException ex)
            {
                await HandleApplicationException(httpContext, ex);
            }
            catch (Exception ex)
            {
                // TO:DO
                // log the original exception
                await HandleApplicationException(httpContext, new Exception("Something went wrong."));
            }
        }

        private async Task HandleApplicationException(
            HttpContext httpContext,
            Exception ex)
        {
            httpContext.Response.ContentType = "application/problem+json";
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            var details = new ValidationProblemDetails();
            details.Detail = ex.Message;
            details.Status = StatusCodes.Status400BadRequest;
            details.Title = "One or more validation errors occurred.";

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(details, settings));
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
