using System.Net;
using System.Text.Json;
using API.Helper;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next,
                                   ILogger<ExceptionMiddleware> logger,
                                   IHostEnvironment env)
        {
            this._next = next;
            this._logger = logger;
            this._env = env;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
             catch (InternalServerError ex)
            {
                _logger.LogError(ex, "Internal Server error");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error");
                await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
            }
            catch (NotFoundException ex)
            {
                _logger.LogError(ex, "Resource not found");
                await HandleExceptionAsync(context, ex, HttpStatusCode.NotFound);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access");
                await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
            }
            catch (ForbiddenAccessException ex)
            {
                _logger.LogError(ex, "Forbidden access");
                await HandleExceptionAsync(context, ex, HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, HttpStatusCode statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ResponseVM(
                flag: true,
                statusCode: context.Response.StatusCode,
                message: _env.IsDevelopment() ? ex.StackTrace : string.Empty,
                response: ex.Message
            );

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(json);
        }
        public class ValidationException : Exception
        {
            public ValidationException(string message) : base(message) { }
        }

        public class NotFoundException : Exception
        {
            public NotFoundException(string message) : base(message) { }
        }

        public class ForbiddenAccessException : Exception
        {
            public ForbiddenAccessException(string message) : base(message) { }
        }
        public class InternalServerError : Exception
        {
            public InternalServerError(string message) : base(message) { }
        }
    }
}