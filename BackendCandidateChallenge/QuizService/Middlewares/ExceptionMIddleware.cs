using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuizService.Errors;
using System;
using System.Net;
using System.Threading.Tasks;

namespace QuizService.Middlewares
{
    public class ExceptionMIddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMIddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.ContentType = "application/json";
            string message = "Internal server error";

            switch (exception)
            {
                // Add your custom exceptions here to manage responses. Example:
                // case ResourceNotFoundException ex:
                //     context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                //     message = ex.Message;
                //     break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(new ErrorDetails { Message = message }.ToString());
        }
    }
}
