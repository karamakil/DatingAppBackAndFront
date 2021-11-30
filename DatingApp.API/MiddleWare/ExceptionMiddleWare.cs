using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using DatingApp.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.MiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleWare> logger;
        private readonly IHostEnvironment env;
        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            this.env = env;
            this.logger = logger;
            this.next = next;
        }

        public async  Task InvokeAsync(HttpContext context)
        {
            try
            {
               await this.next(context);
            }
            catch (System.Exception ex)
            {
                this.logger.LogError(ex,ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var response = env.IsDevelopment()? 
                new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace)
                :new ApiException(context.Response.StatusCode,"Internal server error");

                // var option = new JsonSerializerOptions(PropertyNamingPolicy = JsonNamingPolicy.CamelCase);
                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}