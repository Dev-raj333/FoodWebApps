using System.Net;
using System;
using System.Runtime.CompilerServices;

namespace FoodDonationWebApp.Helper
{
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandleExceptionMiddleware> _logger;
        public HandleExceptionMiddleware(RequestDelegate next, ILogger<HandleExceptionMiddleware> logger) { 
            _next = next;
            _logger = logger;   
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unhandled exception has occurred");
                await HandleExceptionAsync(context, ex);
            }

            
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex) {
            _logger.LogError(ex, "An unhandled exception has occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = System.Text.Json.JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "Internal Server Error. Please try again later.",
                Detailed = ex.Message
            });
            return context.Response.WriteAsync(result);
        }
    }
}
