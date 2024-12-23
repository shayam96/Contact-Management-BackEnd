using Newtonsoft.Json;
using System.Net;

namespace Contact_info_app.Middleware
{
    public class GlobalErrorHandling
    {
        private readonly RequestDelegate _next;
        public GlobalErrorHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new { message = exception.Message };
            var json = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(json);
        }
    }
}
