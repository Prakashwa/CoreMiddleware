using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CoreMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MiddlewareA
    {
        private readonly RequestDelegate _next;

        public MiddlewareA(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await httpContext.Response.WriteAsync("A (before) \n");
            await _next(httpContext);
            await httpContext.Response.WriteAsync("A (afer) \n");
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareA(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareA>();
        }
    }
}
