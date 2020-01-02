using System;
using System.Collections.Generic;
using System.Text;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Data
{
    /// <summary>
    /// dotnet ef migrations remove
    /// # 如果指定了文件夹，之后的修订也需要指定文件夹选项`-o ./Data/Migrations`
    /// dotnet ef migrations add -o./Data/Migrations Init
    /// dotnet ef database update
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
