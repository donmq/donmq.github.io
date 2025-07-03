using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace eTierV2_API.Helpers.Params
{
     public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IConfiguration _configuration;

        public ExceptionHandlingMiddleware(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try {
                await next(context);
            }
            catch(Exception e) 
            {
                _configuration.GetSection("AppSettings:DataSeach").Value = "";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsync(e.Message);
            }
        }
    }
}