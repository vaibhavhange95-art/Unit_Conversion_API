using System.Net;
using System.Text.Json;
using Unit_Conversion_API.Exceptions;

namespace Unit_Conversion_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ConversionException ex)
            {
                context.Response.StatusCode =
                    (int)HttpStatusCode.BadRequest;

                context.Response.ContentType =
                    "application/json";


                var response = new
                {
                    statusCode = 400,
                    message = ex.Message
                };


                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
            catch (Exception)
            {
                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                context.Response.ContentType =
                    "application/json";


                var response = new
                {
                    statusCode = 500,
                    message = "An unexpected error occurred."
                };


                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }
        }
    }
}