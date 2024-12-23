using Newtonsoft.Json;
using Serilog;

namespace Knab.Cryptocurrency.Presentation.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Logger.Error(exception, exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var result = JsonConvert.SerializeObject(new
            {
                Exception = exception.Message,
                exception.StackTrace,
            });

            await context.Response.WriteAsync(result);
        }
    }
}
