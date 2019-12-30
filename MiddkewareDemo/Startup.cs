using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiddkewareDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // RoutingMiddleware Core-v3.0 default added
            //services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            // 采用这种方式的路由时taskApp与app不一致
            app.Map("/task", taskApp =>
            {
                taskApp.Run(async context =>
                {
                    await context.Response.WriteAsync("this is a task");
                });
            });

            app.UseRouting();
            // 在`app.UseRouting();`之后
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
                // `/action`=`action`
                endpoints.MapGet("action", async context =>
                {
                    await context.Response.WriteAsync("this is a action");
                });
            });

            // 手工一步步创建路由
            RequestDelegate handler = context => context.Response.WriteAsync("this is a handler action");
            var route = new Route(new RouteHandler(handler), "action2", app.ApplicationServices.GetRequiredService<IInlineConstraintResolver>());
            app.UseRouter(route);

            // RoutingMiddleware
            app.UseRouter(builder => builder.MapRoute("action3", (context) => context.Response.WriteAsync("this is a action(UseRouter->MapRoute)")));
            app.UseRouter(builder => builder.MapVerb("GET", "action4", (context) => context.Response.WriteAsync("this is a action(UseRouter->MapVerb)")));
            app.UseRouter(builder => builder.MapGet("action5", (context) => context.Response.WriteAsync("this is a action(UseRouter->MapGet)")));

            // Core-v3.0 not Support, Replaced by `app.UseEndpoints(endpoints => {})`
            //app.UseMvc(route => route.MapRoute("ss", "ssss"));

            // Middleware Test
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("1 : before starting...");
                await next();
                //await next.Invoke();
            });
            app.Use(next =>
            {
                return (context) =>
                {
                    context.Response.WriteAsync("2 : in the middle of start...");
                    return next(context);
                    //return next.Invoke(context);
                };
            });
            // Run 结束节点
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("3 : start......");
            });
        }
    }
}
