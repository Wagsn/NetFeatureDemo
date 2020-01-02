using IdentityDemo.Data;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityDemo
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
            // ע������
            services.AddSingleton(Configuration as IConfigurationRoot);
            // ע��JwtSettingsʵ������ Usage: ������ע�� IOptions<JwtSettings> options
            //services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            // SqlServer
            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Pomelo.EntityFrameworkCore.MySql
            services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));

            // options.SignIn.RequireConfirmedAccount ȷ�����䣬Ĭ��false
            services.AddDefaultIdentity<ApplicationUser>(options => { })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            // �Զ����User��Role
            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            //.AddDefaultTokenProviders();

            // requires
            // using Microsoft.AspNetCore.Identity.UI.Services;
            // using IdentityDemo.Services;
            //services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, Services.EmailSender>();
            // ע����Ȩ��Ϣ���������� Usage: ������ע�� IOptions<AuthMessageSenderOptions> options
            //services.Configure<Services.EmailSender.AuthMessageSenderOptions>(Configuration.GetSection("AuthMessageSenderOptions"));

            // ��������֤
            //services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/Account/Login";
            //    });

            services.Configure<IdentityOptions>(options =>
            {
                // ����������Сд��ĸ Default: true
                options.Password.RequireLowercase = false;
                // ��������������ĸ���� Default: true
                options.Password.RequireNonAlphanumeric = false;
                // ������������д��ĸ Default: true
                options.Password.RequireUppercase = false;
                // �������������� Default: true
                options.Password.RequireDigit = false;
                // ���������������ַ������� Default: 1
                options.Password.RequiredUniqueChars = 0;
            });

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            // ��Ȩ
            app.UseAuthentication();
            // ��֤
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
