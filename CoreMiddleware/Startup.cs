using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreMiddleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app)
        {
            //Note: This map is specific to route https://BaseURL/Payment
            app.Map("/Payment", HandlePaymentRoute);


            //Note: This map when is specific to route https://BaseURL/?search=1
            app.MapWhen(context => { return context.Request.Query.ContainsKey("Search"); }, HanldeMapWhenSeachinQS);


            // Middleware A, it is created in separte file and this is best practice
            app.UseMiddlewareA();

            // Middleware B, this is inline approach 
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("B (before) \n");
                await next();
                await context.Response.WriteAsync("B (after) \n");
            });

            // Middleware C (terminal)
            app.Run(async context =>
            {
                await context.Response.WriteAsync("C: Hello world \n");
            });
        }
        private static void HandlePaymentRoute(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Payment processing yet to implement. \n");
            });
        }

        private static void HanldeMapWhenSeachinQS(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync("Calling search");
            });
        }

       
    }
}
