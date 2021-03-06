using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Identity.API.Configurations.Middleware
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null)
                    {
                        //logger here

                        //response
                        var response = new
                        {
                            success = false,
                            Errors = new[] { "Internal Server Error" }
                        };
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                    }
                });

                

            });
        }
    }
}
