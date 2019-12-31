using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JwtAuthDemo
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
#if CoreApp2x
            // Add Authentication (身份认证) by JWT
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            var jwtSettings = new JwtSettings
            {
                Issuser = "jim",
                Audience = "",
                SecretKey = "JwtAuthDemo.SecretKey"
            };
            Configuration.Bind("JwtSettings", jwtSettings);
            services
                .AddAuthentication(options => {
                    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    // JwtBearer Validator
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidIssuer = jwtSettings.Issuser,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };

                    // Custom MyToken Validator
                    //options.SecurityTokenValidators.Clear();
                    //options.SecurityTokenValidators.Add(new MyTokenValidator());
                    //options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                    //{
                    //    OnMessageReceived = context =>
                    //    {
                    //        var token = context.Request.Headers["mytoken"];
                    //        context.Token = token.FirstOrDefault();
                    //        return Task.CompletedTask;
                    //    }
                    //};
                });
            services.AddAuthorization(options =>
            {
                // Claim 授权
                options.AddPolicy("SuperAdminOnly", policy => policy.RequireClaim("SuperAdminOnly"));
            });
            // Add MVC
            services.AddMvc();
#endif

            // Core-v3.0
#if CoreApp30
            //services.AddControllers();
#endif
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Core-v3.0
#if CoreApp30
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#endif
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        // Core-2.x
#if CoreApp2x
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
#endif
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // HTTPS重定向
            //app.UseHttpsRedirection();

            // Use Authentication (身份认证)
            app.UseAuthentication();

            // 允许跨域访问
            //app.UseCors(options => {
            //    options.AllowAnyHeader();
            //    options.AllowAnyMethod();
            //    options.AllowAnyOrigin();
            //    options.AllowCredentials();
            //});

            // Use MVC
            app.UseMvcWithDefaultRoute();
            //app.UseMvc();

#if CoreApp30
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
#endif
        }
    }
}
