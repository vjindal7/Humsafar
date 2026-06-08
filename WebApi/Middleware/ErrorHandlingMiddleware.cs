using Core;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (ex)
            {
                case RestException re:
                    errors = re.Errors;
                    if (logger.IsEnabled(LogLevel.Error)) logger.LogError("REST ERROR: [{Error}]", errors == null ? re.Code.ToString() : errors.ToString());
                    context.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    if (logger.IsEnabled(LogLevel.Error)) logger.LogError("SERVER ERROR: [{Error}]", ex.Message);
                    if (logger.IsEnabled(LogLevel.Error)) logger.LogError("{ExceptionStack}", ex.StackTrace);
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Unknown Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = "application/json";
            if (errors != null)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = errors }));
            }
        }
    }
}