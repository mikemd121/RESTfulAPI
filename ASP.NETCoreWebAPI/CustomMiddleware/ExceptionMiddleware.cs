
using Newtonsoft.Json;
using System.Net;
using System.Web.Http.Filters;

namespace ASP.NETCoreWebAPI.CustomMiddleware
{
    public  class ExceptionMiddleware
    {
        public RequestDelegate _next;
        public  ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;

        }
     
        public  async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessageAsync(httpContext, ex).ConfigureAwait(false); throw;
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }

    }
}
