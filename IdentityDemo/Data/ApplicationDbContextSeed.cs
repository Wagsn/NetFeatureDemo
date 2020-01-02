using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Data
{
    /// <summary>
    /// 数据库种子数据
    /// </summary>
    public class ApplicationDbContextSeed
    {
        private UserManager<ApplicationUser> _userManager;

        public async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
        {
            if (!context.Users.Any())
            {
                _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var defaultUser = new ApplicationUser
                {
                    Email = "wagsn@foxmail.com",
                    //NormalizedEmail = "WAGSN@FOXMAIL.COM",
                    UserName = "wagsn@foxmail.com",
                    //NormalizedUserName = "ADMIN",
                };
                var result = await _userManager.CreateAsync(defaultUser, "123456");
                if (!result.Succeeded)
                {
                    throw new Exception("初始默认用户失败");
                }
            }
        }
    }
}
