﻿using Microsoft.AspNetCore.Mvc;

namespace CustomSoftWebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly IDictionary<string, string[]> errorList = new Dictionary<string, string[]>();
        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
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
                _logger.LogError(ex, "An unhandled exception occurred.");

                var problemDatails = new ValidationProblemDetails()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Unexpected error occurred.",
                    Detail = ex.Message
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(problemDatails);
            }
        }
    }
}
