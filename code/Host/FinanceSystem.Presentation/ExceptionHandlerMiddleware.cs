using Shared.Core;
using Shared.Core.Exceptions;
using System.Net;

namespace FinanceSystem.Presentation
{
    public static class ConfigExceptionHandler
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }


    public class ExceptionHandlerMiddleware(
        RequestDelegate next,
        IStringLocalizerService stringLocalizerService,
        ILoggerService logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly IStringLocalizerService _stringLocalizerService = stringLocalizerService;
        private readonly ILoggerService _logger = logger;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BusinessException ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (exception?.InnerException is BusinessException coreException)
            {
                context.Response.StatusCode = (int)coreException.Code;
                return GenerateResponse(coreException.Code, GetExceptionMessage(coreException), context);
            }
            if (exception is BusinessException businessException)
            {
                context.Response.StatusCode = (int)businessException.Code;
                return GenerateResponse(businessException.Code, GetExceptionMessage(businessException), context);
            }
            _logger.Error(exception, exception.Message);
            return GenerateResponse((int)HttpStatusCode.InternalServerError, "InternalServerError", context);
        }

        private Task GenerateResponse(int code, string message, HttpContext context)
        {
            return context.Response.WriteAsync(JsonResponse<ErrorDetails>.Failure(new ErrorDetails
            {
                Code = code,
                ErrorMessage = message
            }).ToString());
        }

        private string GetExceptionMessage(BusinessException businessException)
        {
            return string.Format(_stringLocalizerService.GetString($"_{businessException.Code}"));
        }
    }

    public class ErrorDetails
    {
        public int Code { get; set; }
        public string ErrorMessage { get; set; }
    }
}
